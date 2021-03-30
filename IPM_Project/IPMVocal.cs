using System;
using System.IO;
using CommonServiceLocator;
using DeepSpeechClient.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace IPM_Project
{
    /// <summary>
    /// Singleton used to run the program.
    /// </summary>
    public class IPMVocal {
        /// <summary>
        /// Singleton's instance. 
        /// </summary>
        private static IPMVocal _instance;
        /// <summary>
        /// TODO.
        /// </summary>
        private VoiceDetector _voiceDetector;
        
        /// <summary>
        /// TODO.
        /// </summary>
        private CommandInterpreter _commandInterpreter;
        
        /// <summary>
        /// TODO.
        /// </summary>
        private RedisIntermediate _redisIntermediate;
        private DeepSpeechClient.DeepSpeech _deepSpeechClient;

        /// <summary>
        /// Constructor
        /// TODO: Doc (what does constructor do?)
        /// Only called once because class is a singleton.
        /// </summary>
        private IPMVocal() {
            InitializeDeepSpeech();
            _voiceDetector = new VoiceDetector(_deepSpeechClient);
            _commandInterpreter = new CommandInterpreter();
            _redisIntermediate = new RedisIntermediate();
        }
        
        /// <summary>
        /// Creates a singleton's instance if it doesn't already exists.
        /// Used to prevent a Singleton from creating multiple instances of itself. 
        /// </summary>
        /// <returns>The Singleton instance</returns>
        public static IPMVocal GetInstance() {
            if (_instance == null)
            {
                _instance = new IPMVocal();
            }
            return _instance;
        }

        /// <summary>
        /// TODO: Doc
        /// </summary>
        private void InitializeDeepSpeech() {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            const int beamWidth = 500;
            _deepSpeechClient = new DeepSpeechClient.DeepSpeech();
            try {
                _deepSpeechClient.CreateModel("..\\..\\output_graph.pbmm", beamWidth);
            } catch (FileNotFoundException Ex){
                Console.Write(Ex.Message);
            }
            

            SimpleIoc.Default.Register<IDeepSpeech>(() => _deepSpeechClient);
            SimpleIoc.Default.Register<VoiceDetector>();
        }
        
        /// <summary>
        /// TODO: Doc
        /// UNUSED.
        /// </summary>
        private void Stop() {
            ServiceLocator.Current.GetInstance<IDeepSpeech>()?.Dispose();
        }
        
    }
}