using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/items")]
    public class ItemsController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Items.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //------
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Items.Get().Where(x => x.Id.ToString().Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Item item = UnitOfWork.Items.Get(id);
            if (item == null) return NotFound();
            return Ok(Factory.Create(item));
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]ItemModel model)
        {
            try
            {
                Item item = Factory.Create(model);
                UnitOfWork.Items.Insert(item);
                UnitOfWork.Commit();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri] int id, [FromBody]ItemModel model)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
        {
            try
            {
                Item item = Factory.Create(model);
                UnitOfWork.Items.Update(item, id);
                UnitOfWork.Commit();
                return Ok(item);
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
                UnitOfWork.Items.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("invoice/{id:int}")]
        public IHttpActionResult GetByInvoice(int id)
        {
            try
            {
                if (UnitOfWork.Invoices.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Items.Get().Where(a => a.Invoice.Id == id).ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
