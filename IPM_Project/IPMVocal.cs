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
        /// Instance of VoiceDetector class
        /// </summary>
        private VoiceDetector _voiceDetector;
        
        /// <summary>
        /// Instance of CommandInterpreter class
        /// </summary>
        private CommandInterpreter _commandInterpreter;
        
        /// <summary>
        /// Instance of RedisIntermediate class
        /// </summary>
        private RedisIntermediate _redisIntermediate;
        private DeepSpeechClient.DeepSpeech _deepSpeechClient;

        private JsonUtils jsonUtils;

        /// <summary>
        /// Constructor
        /// Initialize DeepSpeech, the JSON files and the 3 main classes of the project.
        /// </summary>
        private IPMVocal() {
            jsonUtils = new JsonUtils();
            jsonUtils.InitPathsJSONFile();
            jsonUtils.InitRedisJSONFile();
            var config = jsonUtils.ReadPathsJSONData();
            
            InitializeDeepSpeech(config.DeepSpeechModelPath);
            _commandInterpreter = new CommandInterpreter();
            _redisIntermediate = new RedisIntermediate();
            _voiceDetector = new VoiceDetector(_deepSpeechClient);
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
        /// Initialize DeepSpeech model located in pathToDeepSpeech
        /// </summary>
        /// <param name="modelPath">Path of DeepSpeech's model</param>
        private void InitializeDeepSpeech(string modelPath) {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            const int beamWidth = 500;
            _deepSpeechClient = new DeepSpeechClient.DeepSpeech();
            
            try {
                _deepSpeechClient.CreateModel(modelPath, beamWidth);
            } catch (FileNotFoundException Ex){
                Console.Write(Ex.Message);
            }

            SimpleIoc.Default.Register<IDeepSpeech>(() => _deepSpeechClient);
            SimpleIoc.Default.Register<VoiceDetector>();
        }
        
    }
}