using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    //[TokenAuthorization("user")]
    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Categories.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Category category = UnitOfWork.Categories.Get(id);
                if (category == null) return NotFound();
                return Ok(Factory.Create(category));
            }  //Ovdje ne trebamo else jer je u jednoj liniji
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
            
        }
        //[TokenAuthorization("admin")]
        [Route("")]
        public IHttpActionResult Post(CategoryModel model)
        {

            try
            {
                Category category = Factory.Create(model);
                UnitOfWork.Categories.Insert(category);
                UnitOfWork.Commit();
                return Ok(Factory.Create(category));
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
        //[TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Put(int id, CategoryModel model) {
            try
            {
                Category category = Factory.Create(model);
                UnitOfWork.Categories.Update(category, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(category));
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
        //[TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Delete(int id) {
            try
            {
                UnitOfWork.Categories.Delete(id);
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