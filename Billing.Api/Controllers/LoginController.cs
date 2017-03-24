using Billing.Api.Helpers;
using Billing.Api.Models;
using Billing.Database;
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
        private BillingIdentity identity = new BillingIdentity();

        [Route("api/login")]
        [HttpPost]
        public IHttpActionResult Login(TokenRequestModel request)
        {
            ApiUser apiUser = UnitOfWork.ApiUsers.Get().FirstOrDefault(x => x.AppId == request.ApiKey);
            if (apiUser == null) return NotFound();
            if (Helper.Signature(apiUser.Secret, apiUser.AppId) != request.Signature) return BadRequest("Bad application signature");

            var rawTokenInfo = apiUser.AppId + DateTime.UtcNow.ToString("s");
            var authToken = new AuthToken()
            {
                Token = rawTokenInfo,
                Expiration = DateTime.Now.AddMinutes(20),
                ApiUser = apiUser
            };
            UnitOfWork.Tokens.Insert(authToken);
            UnitOfWork.Commit();
            return Ok(Factory.Create(authToken));
        }

        [Route("api/logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                WebSecurity.Logout();
                return Ok($"User {identity.CurrentUser} logged out");
            }
            else
            {
                return Ok("No user is logged in!!!");
            }

        }
    }
}