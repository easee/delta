using System.Threading;

namespace Billing.Api.Helpers
{
    public class BillingIdentity
    {
        public string CurrentUser
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.Name;

                //var username = Thread.CurrentPrincipal.Identity.Name;
                //UnitOfWork unit = new UnitOfWork();
                //return unit.Agents.Get().FirstOrDefault(x => x.Username == username).Name;
                
            }
        }

        public bool HasRole(string role)
        {
            return Thread.CurrentPrincipal.IsInRole(role);
        }
    }
}