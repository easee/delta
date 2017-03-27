using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using Billing.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebMatrix.WebData;

namespace Billing.Api.Controllers
{
    //[TokenAuthorization("admin,user")]
    [RoutePrefix("api/agents")]
    public class AgentsController : BaseController
    {
        //[TokenAuthorization("user")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Agents.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        //[TokenAuthorization("user")]
        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            return Ok(UnitOfWork.Agents.Get().Where(x => x.Name.Contains(name)).ToList()
                                  .Select(a => Factory.Create(a)).ToList());
        }
        //[TokenAuthorization("user")]
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
            WebSecurity.InitializeDatabaseConnection
                //("Billing", "UserProfile", "UserId", "Username", autoCreateTables: true); ovo je bilo ranije
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
