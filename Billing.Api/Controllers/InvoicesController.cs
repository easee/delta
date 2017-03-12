using Billing.Api.Models;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/invoices")]
    public class InvoicesController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Invoices.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Invoice invoice = UnitOfWork.Invoices.Get(id);
            if (invoice == null) return NotFound();
            return Ok(Factory.Create(invoice));
        }

        [Route("number/{invoiceNo}")]
        public IHttpActionResult Get(string invoiceNo)
        {
            return Ok(UnitOfWork.Invoices.Get().Where(x => x.InvoiceNo.Equals(invoiceNo)).ToList().Select(a => Factory.Create(a)).ToList());
        }

        [Route("")]
        public IHttpActionResult Post([FromBody] InvoiceModel model)
        {
            try
            {
                Invoice invoice= Factory.Create(model);
                UnitOfWork.Invoices.Insert(invoice);
                UnitOfWork.Commit();
                return Ok(Factory.Create(invoice));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri] int id, [FromBody]Invoice invoice)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
        {
            try
            {
                UnitOfWork.Invoices.Update(invoice, id);
                UnitOfWork.Commit();
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                UnitOfWork.Invoices.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
