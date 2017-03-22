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
    public class SalesByCategoryController : BaseController
    {

        //[BillingAuthorization] DODATI NAKNADNO
        public IHttpActionResult Post(RequestModel request)
        {
            {
                try
                {
                    return Ok(SalesByCategoryReport.Report(UnitOfWork, request));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }


        }
    }
}
