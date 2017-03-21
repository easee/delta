using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [BillingAuthorization]
    [RoutePrefix("api/suppliers")]
    public class SuppliersController : BaseController
    {
        //public IBillingRepository<Supplier> suppliers = new BillingRepository<Supplier>(new BillingContext());
        //Factory factory = new Factory();
        //public IHttpActionResult Get()
        //{   return Ok(agents.Get().ToList())

        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Suppliers.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //------
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Suppliers.Get().Where(x => x.Name.Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Supplier supplier = UnitOfWork.Suppliers.Get(id);
            if (supplier == null) return NotFound();
            return Ok(Factory.Create(supplier));
        }


        public IHttpActionResult Post([FromBody] SupplierModel model)
        {
            try
            {
                Supplier supplier = Factory.Create(model);
                UnitOfWork.Suppliers.Insert(supplier);
                UnitOfWork.Commit();
                return Ok(Factory.Create(supplier));
            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");

                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri] int id, [FromBody]SupplierModel model)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
        {
            try
            {
                Supplier supplier = Factory.Create(model);
                UnitOfWork.Suppliers.Update(supplier, id);
                UnitOfWork.Commit();
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");

                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Supplier entity = UnitOfWork.Suppliers.Get(id);
                if (entity == null) return NotFound();
                UnitOfWork.Suppliers.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");

                return BadRequest(ex.Message);
            }
        }




    }
}
