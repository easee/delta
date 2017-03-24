using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Api.Reports;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("admin")]
    public class DashboardController : BaseController
    {
        public IHttpActionResult Get()

        {
            try
            {
                return Ok(Reports.Dashboard.Report());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
