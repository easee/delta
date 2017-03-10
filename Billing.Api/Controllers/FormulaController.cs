using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/formula")]
    public class FormulaController : ApiController
    {
        [Route("empty")]
        public string Get()
        {
            return "Formula GET. Nothing passed.";
        }

        [Route("{x?}")]
        public string Get(string x = "jupiter")
        {
            return string.Format($"Formula GET. String {x} passed.");
        }

        [Route("~/api/matematika/{*x:datetime}")]
        public string Get(DateTime x)
        {
            return string.Format($"Matematika GET. Date {x} passed.");
        }

        [Route("~/api/matematika/{id:int?}")]
        public string Get(int id = 0)
        {
            id *= 3;
            return string.Format($"Matematika GET. Integer {id} passed.");
        }

        [Route("~/api/matematika")]
        public string Post(string param)
        {
            return string.Format($"hey, you just passed a {param}");
        }

        [Route("~/api/matematika")]
        public IHttpActionResult Put(string param)
        {
            return BadRequest(string.Format($"hey, you just passed a {param}"));
        }
    }
}
