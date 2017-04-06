using Billing.Api.Models;
using Billing.Database;
using System.Web.Security;

namespace Billing.Api.Helpers
{
    public static class BillingIdentity
    {
        public static Agent Agent;

        public static CurrentUserModel CurrentUser
        {
            get
            {
                Billing.Database.CurrentUser.Id = Agent.Id;
                CurrentUserModel model = new CurrentUserModel()
                {
                    Id = Agent.Id,
                    Name = Agent.Name,
                    Username = Agent.Username,
                    Roles = Roles.GetRolesForUser(Agent.Username)
                };
                return model;
            }
        }
    }
}