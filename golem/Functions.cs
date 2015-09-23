using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;

namespace golem
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        // Endpoint=sb://pending-room-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=oEuKGLgVlVEIkhCR4B96qz4yOCeqL0YeqzOJ8DVyH4w=
        public static void ProcessQueueMessage([ServiceBusTrigger("pending_room")] BrokeredMessage message,
        TextWriter logger)
        {
            try
            {

                DateTime t_stamp = DateTime.Parse(message.Properties["query_time"].ToString());
                //logger.WriteLine(message.GetBody<string>());
                string roomId = message.GetBody<string>();
                var str = Utility.GetEntities(roomId);
                logger.WriteLine("Processing for room {0} starting", str.Trim());
                var treasure = Utility.GetEntities(roomId);
                message.Complete();
                
            }
            catch(Exception ex)
            {
                // can happen if some asshole fabricate queue data, after stealing service bus credential.
                // I'm feeling like blackhat
            }
        }
    }
}
