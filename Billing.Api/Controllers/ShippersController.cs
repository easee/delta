using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [BillingAuthorization]
    [RoutePrefix("api/shippers")]
    public class ShippersController : BaseController
    {

        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Shippers.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                return Ok(UnitOfWork.Shippers.Get().Where(x => x.Name.Contains(name)).ToList().Select(a => Factory.Create(a)).ToList());
            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");

                return BadRequest(ex.Message);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Shipper shipper = UnitOfWork.Shippers.Get(id);
                if (shipper == null) return NotFound();
                return Ok(Factory.Create(shipper));
            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");

                return BadRequest(ex.Message);
            }
        }
        [Route("")]
        public IHttpActionResult Post([FromBody] ShipperModel model)
        {
            try
            {
                Shipper shipper = Factory.Create(model);
                UnitOfWork.Shippers.Insert(shipper);
                UnitOfWork.Commit();
                return Ok(Factory.Create(shipper));
            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");

                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri] int id, [FromBody]ShipperModel model)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
        {
            try
            {
                Shipper shipper = Factory.Create(model);
                UnitOfWork.Shippers.Update(shipper,id);
                UnitOfWork.Commit();
                return Ok(shipper);
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
                UnitOfWork.Shippers.Delete(id);
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
