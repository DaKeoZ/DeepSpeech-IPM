using System;
using StackExchange.Redis;

namespace IPM_Project
{
    /**
	 * Class used to communicate with Redis server
	 */
    public class RedisIntermediate {
        
		
        private IDatabase Connection;
        private readonly ConnectionMultiplexer Muxer;

        /**
		 * Constructor
		 */
        public RedisIntermediate() {
            
            Muxer = ConnectionMultiplexer.Connect("dakeoz.fr:6379,password=salut");
            Connection = Muxer.GetDatabase();
        }

        /**
         * Sends request to Redis Server
		 * in: CommandType corresponding to the function type
         */
        public async void SendRequest(string request, CommandType commandType) {
            
            try
            {
                if (request == null && commandType.ToString() == null) return;

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