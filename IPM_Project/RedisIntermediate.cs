using System;
using System.IO;
using Newtonsoft.Json;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IPM_Project
{

    /// <summary>
    /// Class used to communicate with Redis server
    /// </summary>
    public class RedisIntermediate {

        private string _redisHost;
        private string _redisPort;
        private string _redisPassword;

        private readonly ConnectionMultiplexer _muxer;

        /// <summary>
        /// Constructor
        /// TODO.
        /// </summary>
        public RedisIntermediate() {

            InitJSONFile();
            ReadJSONData();

            try {
                _muxer = ConnectionMultiplexer.Connect(_redisHost + ":" + _redisPort + ",password=" + _redisPassword);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            
        }

        
        /// <summary>
        /// Sends request to Redis Server
        /// </summary>
        /// <param name="request">TODO.</param>
        /// <param name="CommandType">corresponding to the function type.</param> 
        public async void SendRequest(string request, CommandType commandType) {
            
            try
            {
                if (request == null && commandType.ToString() == null) {
                    return;
                }

                string fullRequest = commandType.ToString() + "::" + request;
					
                var sub = _muxer.GetSubscriber();
                await sub.PublishAsync("requestSent", fullRequest, CommandFlags.FireAndForget);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }

        private void InitJSONFile() {

            RedisConfiguration redisConfiguration = new RedisConfiguration();
            string JSONResult = JsonConvert.SerializeObject(redisConfiguration);
            string path = @"..\\..\\appsettings.json";
            
            if (!File.Exists(path)) {
                using (var file = new StreamWriter(path, true)) {
                    file.WriteLine(JSONResult.ToString());
                    file.Close();
                }
            }

        }

        private void ReadJSONData() {
            
            using (StreamReader r = new StreamReader(@"..\\..\\appsettings.json"))
            {
                string json = r.ReadToEnd();
                RedisConfiguration redisConfiguration = JsonConvert.DeserializeObject<RedisConfiguration>(json);

                _redisHost = redisConfiguration.RedisHost;
                _redisPort = redisConfiguration.RedisPort;
                _redisPassword = redisConfiguration.RedisPassword;
            }

        }
        
    }
}