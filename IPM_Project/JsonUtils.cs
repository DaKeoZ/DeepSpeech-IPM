using System.IO;
using Newtonsoft.Json;

namespace IPM_Project {
    public class JsonUtils {
        
        private string _redisHost;
        private string _redisPort;
        private string _redisPassword;
        
        private string _dsModelPath;
        private string _lmPath;
        private string _triePath;

        private static JsonUtils _instance;

        /// <summary>
        /// Creates a singleton's instance if it doesn't already exists.
        /// Used to prevent a Singleton from creating multiple instances of itself. 
        /// </summary>
        /// <returns>The Singleton instance</returns>
        public static JsonUtils GetInstance() {
            if (_instance == null)
            {
                _instance = new JsonUtils();
            }
            return _instance;
        }
        
        /// <summary>
        /// Initialize JSON File related to paths
        /// </summary>
        public void InitPathsJSONFile() {

            PathsConfiguration pathsConfiguration = new PathsConfiguration();
            string JSONResult = JsonConvert.SerializeObject(pathsConfiguration);
            string path = @"..\\..\\pathsConfig.json";
            
            if (!File.Exists(path)) {
                using (var file = new StreamWriter(path, true)) {
                    file.WriteLine(JSONResult.ToString());
                    file.Close();
                }
            }

        }

        /// <summary>
        /// Read JSON data from the paths config file
        /// </summary>
        public PathsConfiguration ReadPathsJSONData() {
            
            using (StreamReader r = new StreamReader(@"..\\..\\pathsConfig.json"))
            {
                string json = r.ReadToEnd();
                PathsConfiguration pathsConfiguration = JsonConvert.DeserializeObject<PathsConfiguration>(json);

                return pathsConfiguration;
            }

        }

        /// <summary>
        /// Initialize JSON File related to Redis
        /// </summary>
        public void InitRedisJSONFile() {

            RedisConfiguration redisConfiguration = new RedisConfiguration();
            string JSONResult = JsonConvert.SerializeObject(redisConfiguration);
            string path = @"..\\..\\redisConfig.json";
            
            if (!File.Exists(path)) {
                using (var file = new StreamWriter(path, true)) {
                    file.WriteLine(JSONResult.ToString());
                    file.Close();
                }
            }

        }

        /// <summary>
        /// Read JSON data from the Redis config file
        /// </summary>
        public RedisConfiguration ReadRedisJSONData() {
            
            using (StreamReader r = new StreamReader(@"..\\..\\redisConfig.json"))
            {
                string json = r.ReadToEnd();
                RedisConfiguration redisConfiguration = JsonConvert.DeserializeObject<RedisConfiguration>(json);

                return redisConfiguration;
            }

        }
    }
}