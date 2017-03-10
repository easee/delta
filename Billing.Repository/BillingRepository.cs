using Billing.Database;
using System.Data.Entity;
using System.Linq;

namespace Billing.Repository
{
    public class BillingRepository<Entity> : IBillingRepository<Entity> where Entity : class
    {
        protected BillingContext context;
        protected DbSet<Entity> dbSet;//Skup podataka sa kojim ćemo manipulisati

        public BillingRepository(BillingContext _context)//Ovo je konstruktor i njemu se predaje _context kao argument
        {
            context = _context;
            dbSet = context.Set<Entity>();
        }

        public IQueryable<Entity> Get()
        {
            return dbSet;
        }

        public Entity Get(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(Entity entity)//stavili smo virtual da bismo nekad ovu mogli overwrite-at
        {
            dbSet.Add(entity);
        }

        public virtual void Update(Entity entity, int id)
        {
            Entity oldEntity = Get(id);
            context.Entry(oldEntity).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            Entity oldEntity = Get(id);
            dbSet.Remove(oldEntity);
        }

        public bool Commit()
        {
            return (context.SaveChanges() > 0);
        }
    }
}