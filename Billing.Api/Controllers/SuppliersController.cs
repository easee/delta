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
    [RoutePrefix("api/suppliers")]
    public class SuppliersController : BaseController
    {
        [Route("")]
        public IHttpActionResult GetAll(int page = 0)
        {
            int PageSize = 8;
            var query = UnitOfWork.Suppliers.Get().OrderBy(x => x.Id).ToList();
            int TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);

            var returnObject = new
            {
                pageSize = PageSize,
                currentPage = page,
                totalPages = TotalPages,
                size = query.Count,
                suppliersList = query.Skip(PageSize * page).Take(PageSize).Select(x => Factory.Create(x)).ToList()
            };
            return Ok(returnObject);
        }
        [Route("pagination")]
        public IHttpActionResult GetAll(string item, int page = 0)
        {
            if (item == null)
                item = "";
            int PageSize = 8;
            List<Supplier> query = new List<Supplier>();
            if (item.Equals(""))
                query = UnitOfWork.Suppliers.Get().OrderBy(x => x.Id).ToList();
            else
                query = UnitOfWork.Suppliers.Get().Where(x => x.Name.Contains(item) || x.Town.Name.Contains(item) || x.Address.Contains(item)).OrderBy(x => x.Id).ToList();

            int TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);

            var returnObject = new
            {
                pageSize = PageSize,
                currentPage = page,
                totalPages = TotalPages,
                size = query.Count,
                suppliersList = query.Skip(PageSize * page).Take(PageSize).Select(x => Factory.Create(x)).ToList()
            };
            return Ok(returnObject);
        }

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            name = name.ToLower();
            return Ok(UnitOfWork.Suppliers.Get().Where(x => x.Name.Contains(name)).ToList() //Contains znači da Sadrži, i nije case sensitive. 
                                   .OrderBy(x => x.Name.ToLower().IndexOf(name)) //IndexOf je case sensitive.
                                  .Select(a => Factory.Create(a))
                                  .Take(8).ToList());//Uzmi 8 na listu, proizvoljno.
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Supplier supplier = UnitOfWork.Suppliers.Get(id);
            if (supplier == null) return NotFound();
            return Ok(Factory.Create(supplier));
        }

        [TokenAuthorization("admin")]
        [Route("")]
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
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Put(int id,SupplierModel model)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
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
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
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
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
    }
}
