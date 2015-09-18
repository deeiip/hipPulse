using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AtAddon.Utility
{
    public class OauthUtil
    {
        public static string GenerateAuthToken(string id, string secret)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.hipchat.com/v2/oauth/token");

            var postData = "grant_type=client_credentials";
            postData += "&scope=view_messages send_notification";
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(id +
                ":" + secret));
            request.Headers.Add("Authorization", "Basic " + encoded);
            //request.Credentials = new NetworkCredential(value.oauthSecret, value.oauthSecret);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }
    }
}
