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
    //[TokenAuthorization("user")]
    [RoutePrefix("api/procurements")]
    public class ProcurementsController : BaseController
    {
        private int inputStock=0;
        //public IBillingRepository<Procurement> procurements = new BillingRepository<Procurement>(new BillingContext());
        //Factory factory = new Factory();
        [Route("")]
        public IHttpActionResult GetAll(int page=0)
        { 
            int PageSize = 8;
            var query = UnitOfWork.Procurements.Get().OrderBy(x => x.Id).ToList();
            int TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);

            var returnObject = new
            {
                pageSize = PageSize,
                currentPage = page,
                totalPages = TotalPages,
                size = query.Count,
                procurementsList = query.Skip(PageSize * page).Take(PageSize).Select(x => Factory.Create(x)).ToList()
            };
            return Ok(returnObject);
        }

        [Route("pagination")]
        public IHttpActionResult GetAll(string item, int page = 0)
        {
            if (item == null)
                item = "";
            int PageSize = 8;
            List<Procurement> query = new List<Procurement>();
            if (item.Equals(""))
                query = UnitOfWork.Procurements.Get().OrderBy(x => x.Id).ToList();
            else
                query = UnitOfWork.Procurements.Get().Where(x => x.Product.Name.Contains(item) || x.Supplier.Name.Contains(item)).OrderBy(x => x.Id).ToList();

            int TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);

            var returnObject = new
            {
                pageSize = PageSize,
                currentPage = page,
                totalPages = TotalPages,
                size = query.Count,
                procurementsList = query.Skip(PageSize * page).Take(PageSize).Select(x => Factory.Create(x)).ToList()
            };
            return Ok(returnObject);
        }

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

        [TokenAuthorization("admin")]
        [Route("")]
        public IHttpActionResult Post(ProcurementModel model) {
            try
            {
                Procurement procurement = Factory.Create(model);
                inputStock = procurement.Quantity;
                Stock Stock = UnitOfWork.Stocks.Get(procurement.Product.Id);
                Stock.Input += inputStock;
                UnitOfWork.Stocks.Insert(Stock);
                UnitOfWork.Procurements.Insert(procurement);
                UnitOfWork.Commit();
                return Ok(Factory.Create(procurement));
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Put(int id, ProcurementModel model)
        {
            try
            {
                int inputStock2 = 0;
                Procurement procurement = Factory.Create(model);
                inputStock2 = procurement.Quantity;
                Stock Stock = UnitOfWork.Stocks.Get(procurement.Product.Id);
                Stock.Input += inputStock2-inputStock;
                UnitOfWork.Stocks.Update(Stock,procurement.Product.Id);
                UnitOfWork.Procurements.Update(procurement,id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(procurement));

            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
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
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }


        }

    }
}
