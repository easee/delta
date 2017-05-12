using Billing.Database;
using System.Data.Entity;
using System.Linq;

namespace Billing.Repository
{
    public class AgentsRepository : BillingRepository<Agent>
    {
        public AgentsRepository(BillingContext _context) : base(_context) { }

        public override void Insert(Agent entity)
        {
            context.Agents.Add(entity);
            context.ChangeTracker.Entries<Town>().ToList().ForEach(p => p.State = EntityState.Unchanged);
            context.SaveChanges();
        }

        public override void Update(Agent entity, int id)
        {
            Agent oldEntity = Get(id);
            if (oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Towns.Clear();
                oldEntity.Towns = entity.Towns;
                context.ChangeTracker.Entries<Town>().ToList().ForEach(p => p.State = EntityState.Modified);
                context.SaveChanges();
            }
        }
    }
}