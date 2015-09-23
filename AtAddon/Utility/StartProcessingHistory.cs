using HipchatApiV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.ServiceBus.Messaging;

namespace AtAddon.Utility
{
    public class History
    {
        public static void StartProcessingHistory(HipchatClient c, string roomName, string roomid)
        {
            var response = c.ViewRoomHistory(roomName, "recent", "UTC", 0, 1000);
            var ttt = response.Items.ToString();
            string jsonStr = JsonConvert.SerializeObject(response.Items);

            using (var context = new Models.ChimeraEntities())
            {
                var res = context.MESSAGE_STORE.Where(x => x.RoomName == roomName).Select(x=>x.Id).ToArray();
                var currentMessageIds = response.Items.ToArray();
                var newMessages = currentMessageIds.Where(x => !res.Contains(x.Id));
                foreach (var item in newMessages)
                {

                    if(item.From.Trim()== "HipPulse" || item.Message == "/report")
                    {
                        continue;
                    }
                    string file = "";
                    if(item.File!=null)
                    {
                        file = item.File.Url;
                    }
                    else
                    {
                        file = "";
                    }
                    context.MESSAGE_STORE.Add(new Models.MESSAGE_STORE()
                    {
                        Color_ = (int)item.Color,
                        Date = item.Date.ToString(),
                        File = file,
                        From = item.From.ToString().Trim(),
                        Id = item.Id,
                        Links = JsonConvert.SerializeObject(item.Links),
                        Message = item.Message,
                        MessageFormat = (int)item.MessageFormat,
                        Type = (int)item.Type,
                        RoomName = roomid
                    });
                    
                }
                try
                {
                    context.SaveChanges();
                    string connectionString = "Endpoint=sb://pending-room-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=oEuKGLgVlVEIkhCR4B96qz4yOCeqL0YeqzOJ8DVyH4w=";


                    QueueClient Client =
                        QueueClient.CreateFromConnectionString(connectionString, "pending_room");
                    BrokeredMessage message = new BrokeredMessage(roomName.Trim());

                   // Set some addtional custom app-specific properties.
                   message.Properties["query_time"] = DateTime.UtcNow.ToString();

                   // Send message to the queue.
                   Client.Send(message);
                }
                catch(Exception ex)
                {
                    throw new Exception("Can not save to db", ex);
                }
            }
        }
    }
}
