using CommonServiceLocator;
using DeepSpeechClient.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace IPM_Project
{
    /**
     * Singleton running the program
     */
    public class IPMVocal {

        private static IPMVocal Instance;
        private VoiceDetector VoiceDetector;
        private CommandInterpreter CommandInterpreter;
        private RedisIntermediate RedisIntermediate;
        private static DeepSpeechClient.DeepSpeech deepSpeechClient;

        public IPMVocal() {
            Instance = this;
            Start();
            VoiceDetector = new VoiceDetector(deepSpeechClient);
            CommandInterpreter = new CommandInterpreter();
            RedisIntermediate = new RedisIntermediate();
        }

        public static IPMVocal GetInstance() {
            return Instance;
        }

        public static void Start() {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            const int BEAM_WIDTH = 500;
            deepSpeechClient = new DeepSpeechClient.DeepSpeech();
            deepSpeechClient.CreateModel("C:\\IPM_Project\\output_graph.pbmm", BEAM_WIDTH);
            SimpleIoc.Default.Register<IDeepSpeech>(() => deepSpeechClient);
            SimpleIoc.Default.Register<VoiceDetector>();
        }

        private void Stop() {
            ServiceLocator.Current.GetInstance<IDeepSpeech>()?.Dispose();
        }
        
    }
}