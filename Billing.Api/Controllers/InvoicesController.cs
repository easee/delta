using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("user")]
    //CRU za own
    [RoutePrefix("api/invoices")]
    public class InvoicesController : BaseController
    {

        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Invoices.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("customer/{id}")]
        public IHttpActionResult GetByCustomer(int id)
        {
            var customers = UnitOfWork.Invoices.Get().Where(x => x.Customer.Id == id).ToList().Select(x => Factory.Create(x)).ToList();
            if (customers.Count != 0) return Ok(customers);
            return NotFound();
        }

        [Route("agent/{id}")]
        public IHttpActionResult GetByAgent(int id)
        {

            var agents = UnitOfWork.Invoices.Get().Where(x => x.Agent.Id == id).ToList().Select(x => Factory.Create(x)).ToList();
            if (agents.Count != 0) return Ok(agents);
            return NotFound();
        }
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Invoice invoice = UnitOfWork.Invoices.Get(id);
                if (invoice == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(Factory.Create(invoice));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        public IHttpActionResult Post(InvoiceModel model)
        {
            try
            {
                Invoice invoice = Factory.Create(model);
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
        public IHttpActionResult Put(int id, InvoiceModel model)
        {
            try
            {
                Agent current = UnitOfWork.Agents.Get(id);
                if (Thread.CurrentPrincipal.Identity.Name == current.Username || Thread.CurrentPrincipal.IsInRole("admin"))
                {
                    Invoice invoice = Factory.Create(model);
                    UnitOfWork.Invoices.Update(invoice, id);
                    UnitOfWork.Commit();
                    return Ok(Factory.Create(invoice));
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {

                Invoice entity = UnitOfWork.Invoices.Get(id);
                if (entity == null) return NotFound();
                List<Item> items = new List<Item>();
                List<int> itemId = new List<int>();
                items = UnitOfWork.Items.Get().Where(a => a.Invoice.Id == id).ToList();
                foreach (var item in items)
                    itemId.Add(item.Id);

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

        [Route("{id}/next/{cancel}")]
        public IHttpActionResult GetNext(int id, bool cancel = false)
        {
            try
            {
                InvoiceHelper helper = new InvoiceHelper();
                Invoice entity = helper.NextStep(UnitOfWork, id, cancel);
                return Ok(Factory.Create(entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}