using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/invoicereview")]
    public class InvoicesReportPopupController : BaseController
    {
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {

            try
            {
                return Ok(Reports.InvoiceReviewPopup.Report(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}