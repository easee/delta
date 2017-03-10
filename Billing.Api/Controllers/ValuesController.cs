using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    public class ValuesController : ApiController
    {
        public struct Param
        {
            public int Id;
            public string Name;
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return string.Format($"value: {id}");
        }

        public HttpResponseMessage Get(int id, string s)
        {
            if (s.Length > 5)
                return Request.CreateResponse(HttpStatusCode.OK, s);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, s);
        }

        public string Post([FromBody] Param param)
        {
            return string.Format($"Insert {param.Name} ({param.Id})");
        }

        public string Put(int id, string name)
        {
            return string.Format($"Update {name} ({id})");
        }
    }
}
