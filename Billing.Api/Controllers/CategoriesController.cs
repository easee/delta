using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Categories.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Category category = UnitOfWork.Categories.Get(id);
            if (category == null) return NotFound();
            return Ok(Factory.Create(category));
        }
    }
}