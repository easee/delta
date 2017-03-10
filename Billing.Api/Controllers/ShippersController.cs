using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/shippers")]
    public class ShippersController : BaseController
    {

        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Shippers.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Shippers.Get().Where(x => x.Name.Contains(name)).ToList().Select(a => Factory.Create(a)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Shipper shipper = UnitOfWork.Shippers.Get(id);
            if (shipper == null) return NotFound();
            return Ok(Factory.Create(shipper));
        }
    }
}
