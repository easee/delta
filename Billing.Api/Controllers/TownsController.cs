using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/towns")]
    public class TownsController : BaseController
    {

        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Towns.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //------
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Towns.Get().Where(x => x.Name.Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Town town = UnitOfWork.Towns.Get(id);
            if (town == null) return NotFound();
            return Ok(Factory.Create(town));
        }
    }
}
