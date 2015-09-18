using HipchatApiV2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AtAddon.Controllers
{
    public class AnchorController : ApiController
    {
        // GET: api/Anchor
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Anchor/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Anchor
        public void Post([FromBody]dynamic value)
        {
            string roomId = value.item.room.id;
            string roomName = value.item.room.name;
            Guid clientId = value.oauth_client_id;
            using (var context = new Models.ChimeraEntities())
            {
                var res = from t in context.TokenBoxes where t.OauthId == clientId.ToString() select t;
                if(res.Count()!=0)
                {
                    var target = res.First();
                    HipchatClient c = new HipchatClient(target.AuthToken);
                    string message = @"I am collecting all the data. Please give me a moment to understand everything. I'm pinging back in a moment";
                    try
                    {
                        c.SendNotification(roomId, new HipchatApiV2.Requests.SendRoomNotificationRequest() { Message = message });
                        Utility.History.StartProcessingHistory(c, roomName);
                        
                    }
                    catch(Exception ex)
                    {
                        var reAuthKey = Utility.OauthUtil.GenerateAuthToken(target.OauthId, target.OauthSecret);
                        dynamic j = JObject.Parse(reAuthKey);
                        target.AuthToken = j.access_token;
                        context.SaveChanges();
                        c = new HipchatClient(target.AuthToken);
                        c.SendNotification(roomId, new HipchatApiV2.Requests.SendRoomNotificationRequest() { Message = message });
                        Utility.History.StartProcessingHistory(c, roomName);
                    }
                    
                }
            }
        }

        // PUT: api/Anchor/5
        public void Put([FromBody]string value)
        {
        }

        // DELETE: api/Anchor/5
        public void Delete(int id)
        {
        }
    }
}
