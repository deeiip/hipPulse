using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace golem
{
    public class Endpoints
    {
        public static string ApiKey { get { return "9da91dd3616a2ba780e75212d00df0d3238e5568"; } }
        public static string UrlKeyName { get { return "url"; } }
        public static string BaseEP { get { return "http://gateway-a.watsonplatform.net/calls/"; } }



        public static string Sentiment { get { return string.Format( "{0}url/URLGetRankedNamedEntities", BaseEP); } }
        public static string DoEntity(string urlValue)
        {
            string accessUrl = string.Format("{0}?{1}={2}&apikey={3}", Sentiment,
                UrlKeyName, System.Uri.EscapeDataString(urlValue), ApiKey);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(accessUrl);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }

    }
    public class Utility
    {
        public static string GetEntities(string url)
        {
            var result = Endpoints.DoEntity(url);
            return result;
        }
    }
}
