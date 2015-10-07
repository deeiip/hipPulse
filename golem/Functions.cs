using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Text.RegularExpressions;
using HipchatApiV2;

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
                 string authToken = message.Properties["auth_token"].ToString();
                //logger.WriteLine(message.GetBody<string>());
                string roomId = message.GetBody<string>();
                //var str = Utility.GetEntities(roomId);
                //logger.WriteLine("Processing for room {0} starting", roomId.Trim());
                //var treasure = Utility.GetEntities(roomId);
                string treasure = null;
                string jackpot = null;
                string magic = null;
                AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIAINYB73XJJD5MCCNA", "YXE31LiskMC8+tFZw+CwFvPr0Rvk2NRp8HU1XZr2",
                    Amazon.RegionEndpoint.USWest2);


                Parallel.Invoke(
                        () => { treasure = Utility.GetEntities(roomId); },
                        () => { jackpot =  Utility.GetConcept(roomId); },
                        () => { magic = Utility.GetKeyword(roomId); });

                Table dataCache = Table.LoadTable(client, "resultCache");

                Document prevCache = dataCache.GetItem(roomId);

                string fix =
                        Regex.Replace(treasure, @"^\s*$\n", string.Empty, RegexOptions.Multiline)
                        .TrimEnd();
                string fixJack = Regex.Replace(jackpot, @"^\s*$\n", string.Empty, RegexOptions.Multiline)
                        .TrimEnd();
                string fixMagic = Regex.Replace(magic, @"^\s*$\n", string.Empty, RegexOptions.Multiline)
                        .TrimEnd();
                if (prevCache == null)
                {
                    var cache = new Document();
                    cache["roomid"] = roomId;
                    cache["entity"] = fix;
                    cache["concept"] = fixJack;
                    cache["keyword"] = fixMagic;
                    cache["time_stamp"] = t_stamp;
                    dataCache.PutItem(cache);
                }
                else
                {
                    prevCache["entity"] = fix;
                    prevCache["concept"] = fixJack;
                    prevCache["keyword"] = fixMagic;
                    prevCache["time_stamp"] = t_stamp;
                    dataCache.UpdateItem(prevCache);
                }
                string mess = string.Format("I've refreshed the report. Find your detailed report at {0}", roomId);
                HipchatClient c = new HipchatClient(authToken);
                c.SendNotification(roomId, new HipchatApiV2.Requests.SendRoomNotificationRequest() { Message = mess });
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
