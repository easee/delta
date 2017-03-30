using Billing.Api.Helpers;
using Billing.Api.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("user")]
    public class CustomersByCategoryController : BaseController
    {
        public IHttpActionResult Post(RequestModel request)
        {
            try
            {
                return Ok(Reports.CustomersByCategory.Report(request));
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
    }
}
