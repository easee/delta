using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebMatrix.WebData;

//Najnoviji test
namespace Billing.Api.Controllers
{
    [RoutePrefix("api/agents")]
    public class AgentsController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Agents.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //------
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Agents.Get().Where(x => x.Name.Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Agent agent = UnitOfWork.Agents.Get(id);
            if (agent == null) return NotFound();
            return Ok(Factory.Create(agent));
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]AgentModel model)
        {
            try
            {
                Agent agent = Factory.Create(model);
                UnitOfWork.Agents.Insert(agent);
                UnitOfWork.Commit();
                return Ok(Factory.Create(agent));
            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri] int id, [FromBody]AgentModel model)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
        {
            try
            {
                Agent agent = Factory.Create(model);
                UnitOfWork.Agents.Update(agent, id);
                UnitOfWork.Commit();
                return Ok(agent);
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
                UnitOfWork.Agents.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                //Helper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("profiles")]
        [HttpGet]
        public IHttpActionResult CreateProfiles()
        {
            WebSecurity.InitializeDatabaseConnection("Billing", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            foreach (var agent in UnitOfWork.Agents.Get())
            {
                string[] names = agent.Name.Split(' ');
                WebSecurity.CreateUserAndAccount(names[0], "billing", false);
            }
            return Ok("User profiles created");
        }
    }
}
