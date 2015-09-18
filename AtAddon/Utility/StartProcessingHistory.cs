using HipchatApiV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AtAddon.Utility
{
    public class History
    {
        public static void StartProcessingHistory(HipchatClient c, string roomName)
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
                        RoomName = roomName
                    });
                    
                }
                try
                {
                    context.SaveChanges();
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
