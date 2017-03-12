using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class Factory
    {
        private UnitOfWork _unitOfWork;
        public Factory(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private BillingContext context;
        public Factory(BillingContext _context)
        {
            context = _context;
        }

        //All together
        public AgentModel Create(Agent agent)
        {
            return new AgentModel()
            {
                Id = agent.Id,
                Name = agent.Name,
                Towns = agent.Towns.Where(x => x.Customers.Count != 0).Select(x => x.Name).ToList()
            };
        }

        //All together
        public CategoryModel Create(Category category)
        {
            return new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products.Count
            };
        }

        public Category Create(CategoryModel category) {

            return new Category
            {
                Id = category.Id,
                Name = category.Name
            };
        }


        //All together
        public ProductModel Create(Product product)
        {
            return new ProductModel()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category.Name,
                Unit = product.Unit,
                Stock = (product.Stock == null) ? 0 : (int)(product.Stock.Input - product.Stock.Output)
            };
        }

        //Denis
        //Entity to Model
        public SupplierModel Create(Supplier supplier)
        {
            return new SupplierModel()
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Address = supplier.Address,
                Town = supplier.Town.Name
            };

        }

        //Model to Entity
        public Supplier Create(SupplierModel model)
        {
            return new Supplier()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address
                //Fali nam ovdje i iz kojeg grada je Supplier
            };
        }



        //Denis
        public TownModel Create(Town town)
        {
            return new TownModel()
            {
                Id = town.Id,
                Name = town.Name,
                Region = town.Region.ToString(),
                Customers = town.Customers.Select(x => x.Name).ToList(),
                Agents = town.Agents.Select(x => x.Name).ToList()
            };

        }

        //Denis
        public ItemModel Create(Item item)
        {
            return new ItemModel()
            {
                Id = item.Id,
                Quantity = item.Quantity,
                Price = item.Price,
                SubTotal = item.SubTotal
            };
        }

        //Anur
        public ProcurementsModel Create(Procurement procurements)
        {
            return new ProcurementsModel()
            {
                Id = procurements.Id,
                Document = procurements.Document,
                Date = procurements.Date,
                Quantity = procurements.Quantity,
                Price = procurements.Price,
                Product = procurements.Product.Name,
                ProductId = procurements.Product.Id,
                Supplier = procurements.Supplier.Name,
                SupplierId = procurements.Supplier.Id     
            };
        }

        public Procurement Create(ProcurementsModel model) {

            return new Procurement() {
                Id = model.Id,
                Document = model.Document,
                Date = model.Date,
                Quantity = model.Quantity,
                Price = model.Price,
                Product = _unitOfWork.Products.Get(model.ProductId),
                Supplier = _unitOfWork.Suppliers.Get(model.SupplierId)
            };
        }
        //Josip
        public ShipperModel Create(Shipper shipper)
        {
            return new ShipperModel()
            {
                Id = shipper.Id,
                Name = shipper.Name,
                Address = shipper.Address,
                Invoices = shipper.Invoices.Select(x => x.InvoiceNo).ToList()
            };
        }

        //Josip
        public CustomerModel Create(Customer customer)
        {
            return new CustomerModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address,
                Town = customer.Town.Name,
                Invoices = customer.Invoices.Select(x => x.InvoiceNo).ToList(),
                TownId = customer.Town.Id
            };
        }
        
        public Customer Create(CustomerModel model)
        {
            return new Customer()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Town = _unitOfWork.Towns.Get(model.TownId)
            };
        }
        
        //Josip
        public InvoiceModel Create(Invoice invoice)
        {
            return new InvoiceModel()
            {
                Id = invoice.Id,
                InvoiceNo = invoice.InvoiceNo,
                Date = invoice.Date,
                Shipper = invoice.Shipper.Name,
                Customer = invoice.Customer.Name,
                Agent = invoice.Agent.Name,
                Total = invoice.Total,
                Shipping = invoice.Shipping
            };
        }

    }
}

