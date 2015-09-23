using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace golem
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        // Endpoint=sb://pending-room-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=oEuKGLgVlVEIkhCR4B96qz4yOCeqL0YeqzOJ8DVyH4w=
        public static void ProcessQueueMessage([ServiceBusTrigger("pending_room")] string message,
        TextWriter logger)
        {
            logger.WriteLine(message);
        }
    }
}
