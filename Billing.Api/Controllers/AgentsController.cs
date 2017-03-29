using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using Billing.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using WebMatrix.WebData;

namespace Billing.Api.Controllers
{
    //[TokenAuthorization("user")] - dodati RU own
    [RoutePrefix("api/agents")]
    public class AgentsController : BaseController
    {
        
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Agents.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        
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

        //[TokenAuthorization("admin")]
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
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        //[TokenAuthorization("user")]
        [Route("{id}")]
        public IHttpActionResult Put([FromUri] int id, [FromBody]AgentModel model)//FromUri i FromBody možemo i ne moramo pisati, podrazumijeva se.
        {
            try
            {
                Agent current = UnitOfWork.Agents.Get(id);
                //If any of two following values is true it will return true and allow access
                if (Thread.CurrentPrincipal.Identity.Name == current.Username || Thread.CurrentPrincipal.IsInRole("admin"))
                {
                    Agent agent = Factory.Create(model);
                    UnitOfWork.Agents.Update(agent, id);
                    UnitOfWork.Commit();
                    return Ok(agent);
                }
                else
                {
                    return Unauthorized();
                }
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
                UnitOfWork.Agents.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        //[TokenAuthorization("admin")]
        [Route("profiles")]
        [HttpGet]
        public IHttpActionResult CreateProfiles()
        {
            WebSecurity.InitializeDatabaseConnection
                ("Billing", "Agents", "Id", "Username", autoCreateTables: true);
            foreach (var agent in UnitOfWork.Agents.Get().ToList())
            {
                if(string.IsNullOrWhiteSpace(agent.Username))
            {
                string[] names = agent.Name.Split(' ');
                string username = names[0].ToLower();
                agent.Username = username;
                UnitOfWork.Agents.Update(agent, agent.Id);
                UnitOfWork.Commit();
                }
            //WebSecurity.CreateUserAndAccount(names[0], "billing", false); - bilo ranije
            WebSecurity.CreateAccount(agent.Username, "billing", false);
        }
            return Ok("User profiles created");
        }
    }
}
