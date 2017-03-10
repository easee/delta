using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Products.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //------
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Products.Get().Where(x => x.Name.Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }

        [Route("class/{clId:int}")]
        public IHttpActionResult Get(int clId)
        {
            return Ok(UnitOfWork.Products.Get().Where(x => x.Category.Id == clId).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }


        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            Product product = UnitOfWork.Products.Get(id);
            if (product == null) return NotFound();
            return Ok(Factory.Create(product));
        }
    }
}
