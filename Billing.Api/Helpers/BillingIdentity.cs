using System.Threading;

namespace Billing.Api.Helpers
{
    public class BillingIdentity
    {
        public string currentUser
        {
            get
            {
                //return "marlon";
                return Thread.CurrentPrincipal.Identity.Name;
            }
        }
    }
}