using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Api.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [BillingAuthorization]
    public class SalesByRegionController : BaseController
    {
        public IHttpActionResult Post(RequestModel request)
        {
            {
                try
                {
                    return Ok(SalesByRegionReport.Report(UnitOfWork, request));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}