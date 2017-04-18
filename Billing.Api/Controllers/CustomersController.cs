using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/customers")]
    public class CustomersController : BaseController
    {
        [Route("")]
        public IHttpActionResult GetAll(int page = 0)
        {
            int PageSize = 8;
            var query = UnitOfWork.Customers.Get().OrderBy(x => x.Id).ToList();
            int TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);

            var returnObject = new
            {
                pageSize = PageSize,
                currentPage = page,
                totalPages = TotalPages,
                size = query.Count,
                customersList = query.Skip(PageSize * page).Take(PageSize).Select(x => Factory.Create(x)).ToList()
            };
            return Ok(returnObject);
        }

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            name = name.ToLower();
            return Ok(UnitOfWork.Customers.Get().Where(x => x.Name.Contains(name)).ToList()
                                                .OrderBy(x => x.Name.ToLower().IndexOf(name))
                                                .Select(a => Factory.Create(a))
                                                .Take(8).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Customer customer = UnitOfWork.Customers.Get(id);
                if (customer == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(Factory.Create(customer));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        public IHttpActionResult Post(CustomerModel model)
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
        public IHttpActionResult Put(int id, CustomerModel model)
        {
            try
            {
                Customer customer = Factory.Create(model);
                UnitOfWork.Customers.Update(customer, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(customer));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Customer entity = UnitOfWork.Customers.Get(id);
                if (entity == null) return NotFound();
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