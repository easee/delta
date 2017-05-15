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
    [TokenAuthorization("user")]
    [RoutePrefix("api/products")]
    public class ProductsController : BaseController
    {
        [Route("")]
        public IHttpActionResult GetAll(int page = 0)
        {
            int PageSize = 8;
            var query = UnitOfWork.Products.Get().OrderBy(x => x.Id).ToList();
            int TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);
            
            var returnObject = new
            {
                pageSize = PageSize,
                currentPage = page,
                totalPages = TotalPages,
                size=query.Count,
                productsList = query.Skip(PageSize * page).Take(PageSize).Select(x => Factory.Create(x)).ToList()
            };
            return Ok(returnObject);
        }
        [Route("pagination")]
        public IHttpActionResult GetAll(string item, int page = 0)
        {
            if (item == null)
                item = "";
            int PageSize = 8;
            List<Product> query = new List<Product>();
            if (item.Equals(""))
                query = UnitOfWork.Products.Get().OrderBy(x => x.Id).ToList();
            else
                query = UnitOfWork.Products.Get().Where(x => x.Name.Contains(item) || x.Category.Name.Contains(item)).OrderBy(x => x.Id).ToList();

            int TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);

            var returnObject = new
            {
                pageSize = PageSize,
                currentPage = page,
                totalPages = TotalPages,
                size = query.Count,
                productsList = query.Skip(PageSize * page).Take(PageSize).Select(x => Factory.Create(x)).ToList()
            };
            return Ok(returnObject);
        }
        //[Route("")]
        //public IHttpActionResult Get()
        //{
        //    return Ok(UnitOfWork.Products.Get().ToList().Select(x => Factory.Create(x)).ToList());
        //}

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                name = name.ToLower();
                return Ok(UnitOfWork.Products.Get().Where(x => x.Name.Contains(name)).ToList() //Contains znači da Sadrži, i nije case sensitive. 
                                       .OrderBy(x => x.Name.ToLower().IndexOf(name)) //IndexOf je case sensitive.
                                      .Select(a => Factory.Create(a))
                                      .Take(8).ToList());//Uzmi 8 na listu, proizvoljno.
            }

            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
            
        }

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
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
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
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
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
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {

                if (UnitOfWork.Products.Get(id) == null) return NotFound();
                var items = UnitOfWork.Items.Get().Where(x => x.Product.Id == id).ToList();
                var procurements = UnitOfWork.Procurements.Get().Where(x => x.Product.Id == id).ToList();
                foreach (var item in items)
                    UnitOfWork.Items.Delete(item.Id);
                foreach (var procurement in procurements)
                    UnitOfWork.Procurements.Delete(procurement.Id);
                UnitOfWork.Stocks.Delete(id);
                UnitOfWork.Products.Delete(id);
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
