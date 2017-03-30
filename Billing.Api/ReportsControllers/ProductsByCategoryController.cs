using Billing.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/inventory")]
    public class ProductsByCategoryController : BaseController
    {
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return Ok(Reports.ProductsByCategory.Report(id));
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
    }
}
