using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AtAddon.Controllers
{
    public class Frequency
    {
        public string Word { get; set; }
        public int Count { get; set; }
    }

    public class CatFrequency
    {
        public string Cat { get; set; }
        public int Count { get; set; }
    }
    public class Entry
    {
        public string Message { get; set; }
        public DateTime T_S { get; set; }
    }
    public class HeatMapController : ApiController
    {
        // GET: api/HeatMap
        public dynamic Get(string roomid)
        {
            using (var context = new Models.ChimeraEntities())
            {
                var res = from w in context.MESSAGE_STORE where w.RoomName == roomid select new { w.Date, w.Message };
                var resArr = res.ToArray();
                List<Entry> temp = new List<Entry>();
                foreach (var item in resArr)
                {
                    temp.Add(new Entry() { Message = item.Message, T_S = DateTime.Parse(item.Date) });
                }
                var qry = from w in temp group w by w.T_S.Date into g select new { Time = g.Key, Messages = g.ToList() };
                AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIAINYB73XJJD5MCCNA", "YXE31LiskMC8+tFZw+CwFvPr0Rvk2NRp8HU1XZr2",
                                                                Amazon.RegionEndpoint.USWest2);
                Table dataCache = Table.LoadTable(client, "resultCache");
                Document Cache = dataCache.GetItem(roomid);
                dynamic entities = JsonConvert.DeserializeObject(Cache["entity"].ToString());
                Dictionary<string, string> words = new Dictionary<string, string>();
                foreach (var item in entities.entities)
                {
                    words.Add(item.text.ToString(), item.type.ToString());
                }
                Dictionary<DateTime, List<Frequency>> register = new Dictionary<DateTime, List<Frequency>>();
                //Dictionary<DateTime, List<CatFrequency>> catRegister = new Dictionary<DateTime, List<CatFrequency>>();
                foreach (var tarWord in words.Keys)
                {
                    int count = 0;
                    foreach (var item in qry)
                    {
                        count = item.Messages.Count(x =>
                        {
                            return x.Message.ToLower().Contains(tarWord.ToLower());
                        });
                        Frequency f = new Frequency()
                        {
                            Word = tarWord,
                            Count = count
                        };
                        if(register.ContainsKey(item.Time))
                        {
                            register[item.Time].Add(f);
                        }
                        else
                        {
                            register.Add(item.Time, new List<Frequency>());
                            register[item.Time].Add(f);
                        }
                        //if(catRegister.ContainsKey(item.Time))
                        //{
                        //    var prev = catRegister[item.Time];
                        //    //var alreadyExists = prev.Select(x => { return x.Cat == words[tarWord]; });
                        //    var alreadyExists = from t in prev where t.Cat.ToLower() == words[tarWord].ToLower() select t;
                        //    if (alreadyExists.Count()==0)
                        //    {
                        //        catRegister[item.Time].Add(new CatFrequency()
                        //        {
                        //            Cat = words[tarWord].ToLower(),
                        //            Count = 1
                        //        });
                        //    }
                        //    else
                        //    {
                        //        var target = alreadyExists.First();
                        //        target.Count++;
                        //    }
                        //}
                        //else
                        //{
                        //    catRegister.Add(item.Time, new List<CatFrequency>());
                        //    catRegister[item.Time].Add(new CatFrequency()
                        //    {
                        //        Cat = words[tarWord],
                        //        Count = 1
                        //    });
                        //}
                    }

                }
                return new { Status = "OK", wordStat = register };
            }
            return JsonConvert.SerializeObject(new { Status = "Fail" });
        }

        // GET: api/HeatMap/5
        public void Get(string roomid, string cat)
        {
            using (var context = new Models.ChimeraEntities())
            {
                var res = from w in context.MESSAGE_STORE where w.RoomName == roomid select new { w.Date, w.Message };
                var resArr = res.ToArray();
                List<Entry> temp = new List<Entry>();
                foreach (var item in resArr)
                {
                    temp.Add(new Entry() { Message = item.Message, T_S = DateTime.Parse(item.Date) });
                }
                var qry = from w in temp group w by w.T_S.Date into g select new { Time = g.Key, Messages = g.ToList() };
                AmazonDynamoDBClient client = new AmazonDynamoDBClient("AKIAINYB73XJJD5MCCNA", "YXE31LiskMC8+tFZw+CwFvPr0Rvk2NRp8HU1XZr2",
                                                                Amazon.RegionEndpoint.USWest2);
                Table dataCache = Table.LoadTable(client, "resultCache");
                Document Cache = dataCache.GetItem(roomid);
                dynamic entities = JsonConvert.DeserializeObject(Cache["entity"].ToString());
                Dictionary<string, string> words = new Dictionary<string, string>();
                foreach (var item in entities.entities)
                {
                    if (item.type.toLower().Contains(cat.ToLower()))
                    {
                        words.Add(item.text.ToString(), item.type.ToString());
                    }
                }
            }
        }

        // POST: api/HeatMap
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/HeatMap/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/HeatMap/5
        public void Delete(int id)
        {
        }
    }
}
