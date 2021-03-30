using System.ComponentModel;
using Newtonsoft.Json;

namespace IPM_Project {
    public class RedisConfiguration {
        
        public string RedisHost { get; set; }
        public string RedisPort { get; set; }
        public string RedisPassword { get; set; }

        public RedisConfiguration() {
            RedisHost = "localhost";
            RedisPort = "6379";
            RedisPassword = "password";
        }

    }
}