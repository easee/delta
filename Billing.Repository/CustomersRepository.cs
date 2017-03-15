using Billing.Database;
using System.Linq;

namespace Billing.Repository
{
    public class CustomersRepository : BillingRepository<Customer>
    {
        public CustomersRepository(BillingContext context) : base(context) { }

        public override void Update(Customer entity, int id)
        {
            Customer oldEntity = Get(id);
            if (oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Town = entity.Town;
            }
        }
    }
}