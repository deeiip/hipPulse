using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AtAddon.Controllers
{
    public class FrequencyController : ApiController
    {
        // GET: api/Frequency
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Frequency/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Frequency
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Frequency/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Frequency/5
        public void Delete(int id)
        {
        }
    }
}
