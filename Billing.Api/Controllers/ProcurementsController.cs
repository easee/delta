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
    [RoutePrefix("api/procurements")]
    public class ProcurementsController : BaseController
    {
        //public IBillingRepository<Procurement> procurements = new BillingRepository<Procurement>(new BillingContext());
        //Factory factory = new Factory();
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Procurements.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //------
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Procurements.Get().Where(x => x.Product.Name.Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Procurement procurement = UnitOfWork.Procurements.Get(id);
            if (procurement == null) return NotFound();
            return Ok(Factory.Create(procurement));
        }
    }
}
