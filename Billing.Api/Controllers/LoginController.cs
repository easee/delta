using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http;
using WebMatrix.WebData;

namespace Billing.Api.Controllers
{
    [BillingAuthorization]
    public class LoginController : BaseController
    {
        private BillingIdentity identity = new BillingIdentity(new UnitOfWork());

        [Route("api/login")]
        [HttpPost]
        public IHttpActionResult Login(TokenRequestModel request)
        {
            ApiUser apiUser = UnitOfWork.ApiUsers.Get().FirstOrDefault(x => x.AppId == request.ApiKey);
            if (apiUser == null) return NotFound();
            if (Helper.Signature(apiUser.Secret, apiUser.AppId) != request.Signature) return BadRequest("Bad application signature");

            string rawTokenInfo = DateTime.Now.Ticks.ToString() + apiUser.AppId;
            byte[] rawTokenByte = Encoding.UTF8.GetBytes(rawTokenInfo);
            var authToken = new AuthToken()
            {
                Token = Convert.ToBase64String(rawTokenByte),
                Expiration = DateTime.Now.AddMinutes(20),
                ApiUser = apiUser
            };
            CurrentUserModel Identity = new BillingIdentity(UnitOfWork).CurrentUser;
            CurrentUser.Id = Identity.Id;

            UnitOfWork.Tokens.Insert(authToken);
            UnitOfWork.Commit();
            return Ok(Factory.Create(authToken,Identity));
        }

        [Route("api/logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing", "Agents", "Id", "UserName", autoCreateTables: true);
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                WebSecurity.Logout();
                return Ok($"User {identity.CurrentUser} logged out!");
            }
            else
            {
                return Ok("No user is logged in!!!");
            }

        }
    }
}