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
        public static void StartProcessingHistory(HipchatClient c, string roomName, string roomid, string auth)
        {
            var response = c.ViewRoomHistory(roomName, "recent", "UTC", 0, 200);
            var ttt = response.Items.ToString();
            string jsonStr = JsonConvert.SerializeObject(response.Items);

            using (var context = new Models.ChimeraEntities())
            {
                string mes;
                var res = context.MESSAGE_STORE.Where(x => x.RoomName == roomid).Select(x=>x.Id).ToArray();
                var currentMessageIds = response.Items.ToArray();
                var newMessages = currentMessageIds.Where(x => !res.Contains(x.Id));
                foreach (var item in newMessages)
                {
                    if(item.Message.Length > 900)
                    {
                        mes = item.Message.Substring(0, 900);
                    }
                    else
                    {
                        mes = item.Message;
                    }
                    if(item.From.Trim()== "HipPulse" || item.Message == "/report" || 
                        item.From.Trim().ToLower().Contains("Summary For HipChat".ToLower()))
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
                        File = "",
                        From = "",
                        Id = item.Id,
                        Links = ""  ,
                        Message = mes,
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
                    BrokeredMessage message = new BrokeredMessage(roomid.Trim());

                   // Set some addtional custom app-specific properties.
                   message.Properties["query_time"] = DateTime.UtcNow.ToString();
                   message.Properties["auth_token"] = auth;
                   // Send message to the queue.
                   Client.Send(message);
                }
                catch(Exception ex)
                {
                    //throw new Exception("Can not save to db", ex);
                }
            }
        }
    }
}
