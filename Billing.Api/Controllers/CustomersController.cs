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
    [RoutePrefix("api/customers")]
    public class CustomersController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Customers.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Customer customer = UnitOfWork.Customers.Get(id);
            if (customer == null) return NotFound();
            return Ok(Factory.Create(customer));
        }

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Customers.Get().Where(x => x.Name.Contains(name)).ToList().Select(a => Factory.Create(a)).ToList());

        }

        [Route("")]
        public IHttpActionResult Post([FromBody] CustomerModel model)
        {
            try
            {
                Customer customer = Factory.Create(model);
                UnitOfWork.Customers.Insert(customer);
                UnitOfWork.Commit();
                return Ok(Factory.Create(customer));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri] int id, [FromBody]CustomerModel model)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
        {
            try
            {
                Customer customer = Factory.Create(model);
                UnitOfWork.Customers.Update(customer, id);
                UnitOfWork.Commit();
                return Ok(customer);
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
                UnitOfWork.Customers.Delete(id);
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
