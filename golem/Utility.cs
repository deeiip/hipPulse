using Newtonsoft.Json;
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
        public static string SelfBase { get { return "https://hipchatimpulse.azurewebsites.net/"; } }
        public static string ApiKey { get { return "9da91dd3616a2ba780e75212d00df0d3238e5568"; } }
        public static string UrlKeyName { get { return "url"; } }
        public static string BaseEP { get { return "http://gateway-a.watsonplatform.net/calls/"; } }



        public static string Sentiment { get { return string.Format( "{0}url/URLGetRankedNamedEntities", BaseEP); } }
        public static string Keyword { get { return string.Format("{0}url/URLGetRankedKeywords", BaseEP); } }
        public static string Concept { get { return string.Format("{0}url/URLGetRankedConcepts", BaseEP); } }
        


        public static string DoEntity(string urlValue)
        {
            string accessUrl = string.Format("{0}?{1}={2}&apikey={3}&outputMode=json", Sentiment,
                UrlKeyName, System.Uri.EscapeDataString(urlValue), ApiKey);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(accessUrl);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            // Read the content.
            //dynamic d = JsonConvert.DeserializeObject(reader.ReadToEnd());
            //string isGood = JsonConvert.SerializeObject(d);
            //return isGood;
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }


        public static string DoKeyword(string urlValue)
        {
            string accessUrl = string.Format("{0}?{1}={2}&apikey={3}&outputMode=json", Keyword,
                UrlKeyName, System.Uri.EscapeDataString(urlValue), ApiKey);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(accessUrl);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            // Read the content.
            //dynamic d = JsonConvert.DeserializeObject(reader.ReadToEnd());
            //string isGood = JsonConvert.SerializeObject(d);
            //return isGood;
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }

        public static string DoConcept(string urlValue)
        {
            string accessUrl = string.Format("{0}?{1}={2}&apikey={3}&outputMode=json", Concept,
                UrlKeyName, System.Uri.EscapeDataString(urlValue), ApiKey);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(accessUrl);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            // Read the content.
            //dynamic d = JsonConvert.DeserializeObject(reader.ReadToEnd());
            //string isGood = JsonConvert.SerializeObject(d);
            //return isGood;
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }
    }
    public class Utility
    {
        public static string GetEntities(string roomKey)
        {
            var targetUri = string.Format("{0}api/room?name={1}&forAlchemi=true", Endpoints.SelfBase, roomKey);
            var result = Endpoints.DoEntity(targetUri);
            return result;
        }
        public static string GetKeyword(string roomKey)
        {
            var targetUri = string.Format("{0}api/room?name={1}&forAlchemi=true", Endpoints.SelfBase, roomKey);
            var result = Endpoints.DoKeyword(targetUri);
            return result;
        }
        public static string GetConcept(string roomKey)
        {
            var targetUri = string.Format("{0}api/room?name={1}&forAlchemi=true", Endpoints.SelfBase, roomKey);
            var result = Endpoints.DoConcept(targetUri);
            return result;
        }
    }
}
