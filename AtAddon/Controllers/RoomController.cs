using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace AtAddon.Controllers
{
    public class RoomController : ApiController
    {
        // GET: api/Room
        public HttpResponseMessage Get(string name, bool forAlchemi)
        {
            var ret = new StringBuilder();
            using (var context = new Models.ChimeraEntities())
            {
                var res = from m in context.MESSAGE_STORE where m.RoomName == name.Trim() select m.Message;
                foreach (var item in res.Take(150))
                {
                    ret.AppendLine(item.Trim());
                }
            }
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StringContent(ret.ToString());
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment"); //attachment will force download
            //result.Content.Headers.ContentDisposition.FileName = string.Format("room_{0}.csv", name.Trim());
            return result;
        }
        
        // GET: api/Room/5
        public HttpResponseMessage Get(string roomId)
        {
            var ret = new StringBuilder();
            //,Date,From,Message,MessageFormat,Type,File,Links,RoomName
            ret.Append("Id,Color,Date,From,Message,MessageFormat,Type,File,Links,RoomName\n");
            using (var context = new Models.ChimeraEntities())
            {
                var res = from m in context.MESSAGE_STORE where m.RoomName == roomId.Trim() select m;
                foreach (var item in res)
                {
                    ret.AppendFormat("\"{0}\",", item.Id);
                    ret.AppendFormat("\"{0}\",", item.Color_);
                    ret.AppendFormat("\"{0}\",", item.Date);
                    ret.AppendFormat("\"{0}\",", item.From);
                    ret.AppendFormat("\"{0}\",", item.Message );
                    ret.AppendFormat("\"{0}\",", item.MessageFormat );
                    ret.AppendFormat("\"{0}\",", item.Type );
                    ret.AppendFormat("\"{0}\",", item.File );
                    ret.AppendFormat("\"{0}\",", item.Links );
                    ret.AppendFormat("\"{0}\"\r\n", roomId );
                }
            }
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StringContent(ret.ToString());
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment"); //attachment will force download
            result.Content.Headers.ContentDisposition.FileName = string.Format("room_{0}.csv", roomId.Trim());
            return result;
        }

        //// POST: api/Room
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Room/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Room/5
        //public void Delete(int id)
        //{
        //}
    }
}
