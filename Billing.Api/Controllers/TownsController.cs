﻿using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    //[TokenAuthorization("user")]
    [RoutePrefix("api/towns")]
    public class TownsController : BaseController
    {

        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Towns.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }
        
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            name = name.ToLower();
            return Ok(UnitOfWork.Towns.Get().Where(x => x.Name.Contains(name)).ToList() //Contains znači da Sadrži, i nije case sensitive. 
                                   .OrderBy(x => x.Name.ToLower().IndexOf(name)) //IndexOf je case sensitive.
                                  .Select(a => Factory.Create(a))
                                  .Take(8).ToList());//Uzmi 8 na listu, proizvoljno.
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Town town = UnitOfWork.Towns.Get(id);
            if (town == null) return NotFound();
            return Ok(Factory.Create(town));
        }

        //[TokenAuthorization("admin")]
        [Route("")]
        public IHttpActionResult Post([FromBody]Town town)
        {
            try
            {
                UnitOfWork.Towns.Insert(town);
                UnitOfWork.Commit();
                return Ok(town);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        //[TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Put([FromUri] int id, [FromBody]Town town)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
        {
            try
            {
                UnitOfWork.Towns.Update(town, id);
                UnitOfWork.Commit();
                return Ok(town);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        //[TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                UnitOfWork.Towns.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
    }
}
