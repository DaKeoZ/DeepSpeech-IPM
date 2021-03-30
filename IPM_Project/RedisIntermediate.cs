using System;
using StackExchange.Redis;

namespace IPM_Project
{

    /// <summary>
    /// Class used to communicate with Redis server
    /// </summary>
    public class RedisIntermediate {
        
		
        private IDatabase Connection;
        private readonly ConnectionMultiplexer Muxer;


        /// <summary>
        /// Constructor
        /// TODO.
        /// </summary>
        public RedisIntermediate() {
            
            Muxer = ConnectionMultiplexer.Connect("dakeoz.fr:6379,password=salut");
            Connection = Muxer.GetDatabase();
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
					
                var sub = Muxer.GetSubscriber();
                await sub.PublishAsync("requestSent", fullRequest, CommandFlags.FireAndForget);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }
        
    }
}