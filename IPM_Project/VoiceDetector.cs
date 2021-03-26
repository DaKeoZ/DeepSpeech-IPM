using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundIn;
using CSCore.Streams;
using DeepSpeechClient.Interfaces;
using DeepSpeechClient.Models;
using GalaSoft.MvvmLight.Command;
using Mvvm;

namespace IPM_Project
{
    public class VoiceDetector : BindableBase {
        
        private const int SampleRate = 16000;
        private const string LMPath = "lm.binary";
        private const string TriePath = "trie";

        private readonly IDeepSpeech _sttClient;
        
        private string CommandSTT;
        
        public IAsyncCommand InferenceFromFileCommand { get; private set; }
        public RelayCommand StartRecordingCommand { get; private set; }
        public IAsyncCommand StopRecordingCommand { get; private set; }
        
        private WasapiCapture _audioCapture;
        private SoundInSource _soundInSource;
        private IWaveSource _convertedSource;
        private ConcurrentQueue<short[]> _bufferQueue = new ConcurrentQueue<short[]>();
        private int _threadSafeBoolBackValue = 0;
        public bool StreamingIsBusy
        {
            get => (Interlocked.CompareExchange(ref _threadSafeBoolBackValue, 1, 1) == 1);
            set
            {
                if (value) Interlocked.CompareExchange(ref _threadSafeBoolBackValue, 1, 0);
                else Interlocked.CompareExchange(ref _threadSafeBoolBackValue, 0, 1);
            }
        }
        
        
        private bool _enableStartRecord;
        public bool EnableStartRecord
        {
            get => _enableStartRecord;
            set => SetProperty(ref _enableStartRecord, value);
        }

        private bool _stopRecordStopRecord;
        
        public bool EnableStopRecord
        {
            get => _stopRecordStopRecord;
            set => SetProperty(ref _stopRecordStopRecord, value,
                onChanged: ()=> ((AsyncCommand)StopRecordingCommand).RaiseCanExecuteChanged());
        }
        
        private MMDevice _selectedDevice;
        public MMDevice SelectedDevice
        {
            get => _selectedDevice;
            set => SetProperty(ref _selectedDevice, value, 
                onChanged: UpdateSelectedDevice);
        }
        
        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private bool _isRunningInference;
        private bool IsRunningInference
        {
            get => _isRunningInference;
            set => SetProperty(ref _isRunningInference, value,
                   onChanged: () => ((AsyncCommand)InferenceFromFileCommand).RaiseCanExecuteChanged());
        }

        private string _transcription;
        public string Transcription
        {
            get => _transcription;
            set => SetProperty(ref _transcription, value);
        }

        private string _audioFilePath;
        public string AudioFilePath
        {
            get => _audioFilePath;
            set => SetProperty(ref _audioFilePath, value);
        }

        private ObservableCollection<MMDevice> _deviceNames;
        public ObservableCollection<MMDevice> AvailableRecordDevices
        {
            get => _deviceNames;
            set => SetProperty(ref _deviceNames, value);
        }
        

        public VoiceDetector(IDeepSpeech sttClient) {
            _sttClient = sttClient;

            InferenceFromFileCommand = new AsyncCommand(ExecuteInferenceFromFileAsync,
                _ => !IsRunningInference);
            
            StartRecordingCommand = new RelayCommand(StartRecording,
                canExecute: CanExecuteStartRecording);

            StopRecordingCommand = new AsyncCommand(StopRecordingAsync,
                _ => EnableStopRecord);

            LoadAvailableCaptureDevices();
        }
        
        private void UpdateSelectedDevice()
        {
            ReleaseCapture();
            InitializeAudioCapture();
        }
        
        private void ReleaseCapture()
        {
            if (_audioCapture != null)
            {
                _audioCapture.DataAvailable -= Capture_DataAvailable;
                _audioCapture.Dispose();
            }
        }
        
        private bool CanExecuteStartRecording() =>
            SelectedDevice != null;

        private void LoadAvailableCaptureDevices()
        {
            AvailableRecordDevices = new ObservableCollection<MMDevice>(
                MMDeviceEnumerator.EnumerateDevices(DataFlow.All, DeviceState.Active)); 
            EnableStartRecord = true;
            if (AvailableRecordDevices?.Count != 0)
                SelectedDevice = AvailableRecordDevices[0];
        }
        
        private void InitializeAudioCapture()
        {
            if (SelectedDevice != null)
            {
                _audioCapture = SelectedDevice.DataFlow == DataFlow.Capture ?
                    new WasapiCapture() : new WasapiLoopbackCapture();
                _audioCapture.Device = SelectedDevice;
                _audioCapture.Initialize();
                _audioCapture.DataAvailable += Capture_DataAvailable;
                _soundInSource = new SoundInSource(_audioCapture) { FillWithZeros = false };
                _convertedSource = _soundInSource
                   .ChangeSampleRate(SampleRate)
                   .ToSampleSource()
                   .ToWaveSource(16);

                _convertedSource = _convertedSource.ToMono();
            } 
        }

        private void Capture_DataAvailable(object sender, DataAvailableEventArgs e)
        {
            byte[] buffer = new byte[_convertedSource.WaveFormat.BytesPerSecond / 2];

            int read;
            while ((read = _convertedSource.Read(buffer, 0, buffer.Length)) > 0)
            {
                short[] sdata = new short[(int)Math.Ceiling(Convert.ToDecimal(read / 2))];
                Buffer.BlockCopy(buffer, 0, sdata, 0, read);
                _bufferQueue.Enqueue(sdata);
                Task.Run(() => OnNewData());
            }
        }
        
        private void OnNewData()
        {
            while (!StreamingIsBusy && !_bufferQueue.IsEmpty)
            {
                if (_bufferQueue.TryDequeue(out short[] buffer))
                {
                    StreamingIsBusy = true;
                    _sttClient.FeedAudioContent(buffer, Convert.ToUInt32(buffer.Length));
                    StreamingIsBusy = false;
                }
            }
        }
        public async Task EnableLanguageModelAsync(string lmPath, string triePath)
        {
            try
            {
                StatusMessage = "Loading language model...";
                const float LM_ALPHA = 0.75f;
                const float LM_BETA = 1.85f;
                await Task.Run(() => _sttClient.EnableDecoderWithLM(LMPath, TriePath, LM_ALPHA, LM_BETA));
                StatusMessage = "Language model loaded.";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
        }
        
        public async Task ExecuteInferenceFromFileAsync()
        {
            try
            {
                IsRunningInference = true;
                Transcription = string.Empty;
                StatusMessage = "Running inference...";
                Stopwatch watch = new Stopwatch();
                var waveBuffer = new NAudio.Wave.WaveBuffer(File.ReadAllBytes(AudioFilePath));
                using (var waveInfo = new NAudio.Wave.WaveFileReader(AudioFilePath))
                {
                    watch.Start();
                    string speechResult = await Task.Run(() => _sttClient.SpeechToText(
                        waveBuffer.ShortBuffer,
                        Convert.ToUInt32(waveBuffer.MaxSize / 2)));

                    watch.Stop();
                    Transcription = $"Audio duration: {waveInfo.TotalTime.ToString()} {Environment.NewLine}" +
                        $"Inference took: {watch.Elapsed.ToString()} {Environment.NewLine}" +
                        $"Recognized text: {speechResult}";
                }
                waveBuffer.Clear();
                StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            finally
            {
                IsRunningInference = false;
            }
        }
        private async Task StopRecordingAsync()
        {
            EnableStopRecord = false;
            _audioCapture.Stop();
            while (!_bufferQueue.IsEmpty && StreamingIsBusy)
            {
                await Task.Delay(90);
            }
            Transcription = _sttClient.FinishStream();
            EnableStartRecord = true;
        }
        
        private void StartRecording()
        {
            _sttClient.CreateStream();
            _audioCapture.Start();
            EnableStartRecord = false;
            EnableStopRecord = true;
        }

    }
}