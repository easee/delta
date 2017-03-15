using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Products.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //------
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                return Ok(UnitOfWork.Products.Get().Where(x => x.Name.Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        //[Route("class/{clId:int}")]
        //public IHttpActionResult Get(int clId)
        //{
        //    try
        //    {
        //        return Ok(UnitOfWork.Products.Get().Where(x => x.Category.Id == clId).ToList()
        //                          .Select(a => Factory.Create(a)).ToList());
        //    }

        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Product product = UnitOfWork.Products.Get(id);
                if (product == null) return NotFound();
                return Ok(Factory.Create(product));
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        public IHttpActionResult Post(ProductModel model)
        {
            try
            {
                Product product = Factory.Create(model);
                UnitOfWork.Products.Insert(product);
                UnitOfWork.Commit();
                return Ok(Factory.Create(product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put(int id, ProductModel model)
        {
            try
            {
                Product product = Factory.Create(model);
                UnitOfWork.Products.Update(product, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(product));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (UnitOfWork.Products.Get(id) == null) return NotFound();
                UnitOfWork.Stocks.Delete(id);
                UnitOfWork.Products.Delete(id);
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
