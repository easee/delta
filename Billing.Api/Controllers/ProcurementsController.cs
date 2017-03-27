﻿using Billing.Api.Helpers;
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
    [TokenAuthorization("admin, user")]
    [RoutePrefix("api/procurements")]
    public class ProcurementsController : BaseController
    {
        //public IBillingRepository<Procurement> procurements = new BillingRepository<Procurement>(new BillingContext());
        //Factory factory = new Factory();
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Procurements.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //------
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Procurements.Get().Where(x => x.Product.Name.Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }

        [Route("doc/{doc}")]
        public IHttpActionResult GetByDoc(string doc)
        {
            return Ok(UnitOfWork.Procurements.Get().Where(x => x.Document == doc).ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("product/{id}")]
        public IHttpActionResult GetByProduct(int id)
        {
            return Ok(UnitOfWork.Procurements.Get().Where(x => x.Product.Id == id).ToList().Select(x => Factory.Create(x)).ToList());
        }



        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Procurement procurement = UnitOfWork.Procurements.Get(id);
            if (procurement == null) return NotFound();
            return Ok(Factory.Create(procurement));
        }
        //Denis: Renamed ProcurementModel name according to conventions.
        //Previous name and alert set by Anur. 
        [Route("")]
        public IHttpActionResult Post(ProcurementModel model) {
            try
            {
                Procurement procurement = Factory.Create(model);
                UnitOfWork.Procurements.Insert(procurement);
                UnitOfWork.Commit();
                return Ok(Factory.Create(procurement));
            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");

                return BadRequest(ex.Message);
            }
        }

       [Route("{id}")]
       public IHttpActionResult Put(int id, ProcurementModel model)
        {
            try
            {
                Procurement procurement = Factory.Create(model);
                UnitOfWork.Procurements.Update(procurement,id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(procurement));

            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");

                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id) {
            try
            {
                if (UnitOfWork.Procurements.Get(id) == null) return NotFound();
                UnitOfWork.Procurements.Delete(id);
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
