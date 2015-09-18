using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HipchatApiV2;
using HipchatApiV2.Responses;
using HipchatApiV2.Enums;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AtAddon.Controllers
{
    public class OAuthRes
    {
        public string capabilitiesUrl;
        public string oauthId;
        public string oauthSecret;
    }
    public class InstallController : ApiController
    {
        // GET: api/Install
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Install/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Install
        public HttpResponseMessage Post([FromBody]OAuthRes value)
        {
            //var a = value;
            //string url = string.Format("https://api.hipchat.com/v2/oauth/token?auth_token={0}&auth_test=true", value.oauthId);
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(value.oauthId + 
            //    ":" + value.oauthSecret));
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();




            try
            {
                var responseString = Utility.OauthUtil.GenerateAuthToken(value.oauthId, value.oauthSecret);
                dynamic j = JObject.Parse(responseString);
                using (var context = new Models.ChimeraEntities())
                {
                    context.TokenBoxes.Add(new Models.TokenBox()
                    {
                        AuthToken = j.access_token,
                        OauthId = Guid.Parse(value.oauthId).ToString(),
                        OauthSecret = value.oauthSecret.Trim()
                    });
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {

                HipchatClient c = new HipchatClient();
                List<TokenScope> l = new List<TokenScope>();
                l.Add( TokenScope.ViewMessages);
                var res = c.GenerateToken(GrantType.ClientCredentials, l, value.oauthId, null, null, value.oauthSecret);
            }
            HttpResponseMessage actRes = Request.CreateResponse(HttpStatusCode.OK);
            return actRes;
        }
        
        
        // PUT: api/Install/5
        public void Put([FromBody]string value)
        {
        }

        // DELETE: api/Install/5
        public void Delete(int id)
        {
        }
    }
}
