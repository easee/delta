using Billing.Api.Models;
using Billing.Database;
using Billing.Repository;
using System.Linq;
using System.Threading;
using System.Web.Security;

namespace Billing.Api.Helpers
{
    public class BillingIdentity
    {
        private UnitOfWork _unitOfWork;

        public BillingIdentity(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CurrentUserModel CurrentUser
        {
            get
            {
                string username = Thread.CurrentPrincipal.Identity.Name;
                if (string.IsNullOrEmpty(username)) username = "marlon";
                Agent agent = _unitOfWork.Agents.Get().FirstOrDefault(x => x.Username == username);
                return new CurrentUserModel()
                {
                    Id = agent.Id,
                    Name = agent.Name,
                    Role = GetRoles()
                };
            }
        }

        public string GetRoles()
        {
            string Roles = HasRole("user") ? "user" : "";
            Roles += HasRole("admin") ? ",admin" : "";
            return Roles;
        }

        public bool HasRole(string role)
        {
            return Thread.CurrentPrincipal.IsInRole(role);
        }
    }
}