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

        private readonly ConnectionMultiplexer _muxer;

        /// <summary>
        /// Constructor
        /// Initialize JSON and connects to redis host.
        /// </summary>
        public RedisIntermediate() {

            var config = JsonUtils.GetInstance().ReadRedisJSONData();
            
            try {
                _muxer = ConnectionMultiplexer.Connect(config.RedisHost + ":" + config.RedisPort + ",password=" + config.RedisPassword);
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            
        }

        
        /// <summary>
        /// Sends request to Redis Server
        /// </summary>
        /// <param name="request">Request associated to the commandtype.</param>
        /// <param name="CommandType">corresponding to the function type.</para 
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

        
        
    }
}