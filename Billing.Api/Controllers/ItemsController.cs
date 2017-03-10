using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/items")]
    public class ItemsController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Items.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //------
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Items.Get().Where(x => x.Id.ToString().Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Item item = UnitOfWork.Items.Get(id);
            if (item == null) return NotFound();
            return Ok(Factory.Create(item));
        }
    }
}
