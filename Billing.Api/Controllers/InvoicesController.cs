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
            try
            {
                Invoice invoice = UnitOfWork.Invoices.Get(id);
                if (invoice == null) return NotFound();
                return Ok(Factory.Create(invoice));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("customer/{id:int}")]
        public IHttpActionResult GetByCustomerId(int id)
        {
            try
            {
                if (UnitOfWork.Customers.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Invoices.Get().Where(a => a.Customer.Id==id).ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("agent/{id:int}")]
        public IHttpActionResult GetByAgentId(int id)
        {
            try
            {
                if (UnitOfWork.Agents.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Invoices.Get().Where(a => a.Agent.Id == id).ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("number/{invoiceNo}")]
        public IHttpActionResult Get(string invoiceNo)
        {
            try
            {
                return Ok(UnitOfWork.Invoices.Get().Where(x => x.InvoiceNo.Equals(invoiceNo)).ToList().Select(a => Factory.Create(a)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        public IHttpActionResult Post([FromBody] InvoiceModel model)
        {
            try
            {
                Invoice invoice= Factory.Create(model);
                UnitOfWork.Invoices.Insert(invoice);
                UnitOfWork.Invoices.Commit();
                return Ok(Factory.Create(invoice));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri] int id, InvoiceModel model)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
        {
            try
            {
                Invoice invoice = Factory.Create(model);
                UnitOfWork.Invoices.Update(invoice, id);
                UnitOfWork.Invoices.Commit();
                return Ok(Factory.Create(invoice));
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
                
                if (UnitOfWork.Invoices.Get(id) == null) return NotFound();
                List<Item> items = new List<Item>();
                List<int> itemId = new List<int>();
                items = UnitOfWork.Items.Get().Where(a => a.Invoice.Id == id).ToList();
                foreach(var item in items)
                {
                    ItemModel model = new ItemModel();
                    itemId.Add(model.Id);
                }
                foreach (int d in itemId)
                    UnitOfWork.Items.Delete(d);

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
