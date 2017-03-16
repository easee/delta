using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly BillingContext _context = new BillingContext();

        private IBillingRepository<Agent> _agents;
        private IBillingRepository<Category> _categories;
        private InvoicesRepository _invoices;
        private IBillingRepository<Item> _items;
        private ProcurementsRepository _procurements;
        private ProductsRepository _products;
        private CustomersRepository _customers;
        private ShippersRepository _shippers;
        private IBillingRepository<Supplier> _suppliers;
        private IBillingRepository<Town> _towns;
        private IBillingRepository<Stock> _stocks;

        public BillingContext Context { get { return _context; } }

        public IBillingRepository<Agent> Agents
        {
            get
            { 
                return _agents ?? (_agents = new BillingRepository<Agent>(_context));//ako su null napravi ih i vrati ih
            }
            //Ovo gore se isto može napisati i:
            //get
            //{
            //    if (_agents == null)
            //    {
            //        _agents = new BillingRepository<Agent>(_context);
            //    }
            //    return _agents;
            //}
        }
        public IBillingRepository<Stock> Stocks { get { return _stocks ?? (_stocks = new BillingRepository<Stock>(_context)); } }
        public IBillingRepository<Category> Categories { get { return _categories ?? (_categories = new BillingRepository<Category>(_context)); } }
        public IBillingRepository<Customer> Customers { get { return _customers ?? (_customers = new BillingRepository<Customer>(_context)); } }
        public InvoicesRepository Invoices { get { return _invoices ?? (_invoices = new InvoicesRepository(_context)); } }
        public IBillingRepository<Item> Items { get { return _items ?? (_items = new BillingRepository<Item>(_context)); } }
        public ProcurementsRepository Procurements { get { return _procurements ?? (_procurements = new ProcurementsRepository(_context)); } }
        public ProductsRepository Products { get { return _products ?? (_products = new ProductsRepository(_context)); } }
        public CustomersRepository Customers { get { return _customers ?? (_customers = new CustomersRepository(_context)); } }
        public ShippersRepository Shippers { get { return _shippers ?? (_shippers = new ShippersRepository(_context)); } }
        public IBillingRepository<Supplier> Suppliers { get { return _suppliers ?? (_suppliers = new BillingRepository<Supplier>(_context)); } }
        public IBillingRepository<Town> Towns { get { return _towns ?? (_towns = new BillingRepository<Town>(_context)); } }
        public void Commit()
        {
            _context.SaveChanges(); //UnitOfWork nam omogućava da imamo ovaj jedan commit. 
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}