using Billing.Database;
using Billing.Api.Helpers;
using Billing.Repository;
using Billing.Seed;
using System;
using System.Collections;
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
                Username = agent.Username,
                Towns = agent.Towns.Where(x => x.Customers.Count != 0).Select(x => x.Name).ToList()
            };
        }

        public Agent Create(AgentModel model)
        {
            return new Agent()
            {
                Id = model.Id,
                Name = model.Name,
                Username = model.Username
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
                CategoryId = product.Category.Id,
                Unit = product.Unit,
                Input = (product.Stock != null) ? product.Stock.Input : 0,
                Output = (product.Stock != null) ? product.Stock.Output : 0,
                Inventory = (product.Stock != null) ? product.Stock.Inventory : 0
            };
        }

        public Product Create(ProductModel model)
        {
            Stock stock = _unitOfWork.Stocks.Get(model.Id);
            if (stock != null)
            {
                stock.Input = model.Input;
                stock.Output = model.Output;
            }
            return new Product()
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Unit = model.Unit,
                Category = _unitOfWork.Categories.Get(model.CategoryId),
                Stock = (stock != null) ? stock : null
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
                Town = supplier.Town.Zip + " " + supplier.Town.Name, //Kod ispisa grada odmah ispiši Zip i Naziv zajedno
                TownId = supplier.Town.Id

            };

        }

        //Model to Entity
        public Supplier Create(SupplierModel model)
        {
            return new Supplier()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Town = _unitOfWork.Towns.Get(model.TownId)
            };
        }

        //Creating Model
        public TownModel Create(Town town)
        {
            return new TownModel()
            {
                Id = town.Id,
                Name = town.Name,
                Region = town.Region.ToString(),
                Zip = town.Zip,
                Customers = town.Customers.Select(x => x.Name).ToList(),
                Agents = town.Agents.Select(x => x.Name).ToList()
            };
        }

        //Model to Entity
        public Town Create(TownModel model)
        {
            return new Town()
            {
                Id = model.Id,
                Name = model.Name,
                Zip = model.Zip
            };
        }
 
        public ItemModel Create(Item item)
        {
            return new ItemModel()
            {
                Id = item.Id,
                Invoice = item.Invoice.InvoiceNo,
                InvoiceId = item.Invoice.Id,
                Product = item.Product.Name,
                Unit = item.Product.Unit,
                ProductId = item.Product.Id,
                Price = item.Price,
                Quantity = item.Quantity,
                SubTotal = item.SubTotal
            };
        }

        public Item Create(ItemModel model)
        {
            return new Item()
            {
                Id = model.Id,
                Invoice = _unitOfWork.Invoices.Get(model.InvoiceId),
                Product = _unitOfWork.Products.Get(model.ProductId),
                Price = model.Price,
                Quantity = model.Quantity
            };
        }

        //Anur
        public ProcurementModel Create(Procurement procurement)
        {
            return new ProcurementModel()
            {
                Id = procurement.Id,
                Document = procurement.Document,
                Date = procurement.Date,
                Quantity = procurement.Quantity,
                Price = procurement.Price,
                Product = procurement.Product.Name,
                ProductId = procurement.Product.Id,
                Supplier = procurement.Supplier.Name,
                SupplierId = procurement.Supplier.Id     
            };
        }

        public Procurement Create(ProcurementModel model)
        {
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
                Town=shipper.Town.Name,
                TownId=shipper.Town.Id,
                Invoices = shipper.Invoices.Select(x => x.InvoiceNo).ToList()
            };
        }

        public Shipper Create(ShipperModel model)
        {
            return new Shipper()
            {
                Id=model.Id,
                Name=model.Name,
                Address=model.Address,
                Town=_unitOfWork.Towns.Get(model.TownId)
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
                ShippedOn = (DateTime)invoice.ShippedOn,
                Status = (int)invoice.Status,
                Vat = invoice.Vat,
                SubTotal = invoice.SubTotal,
                Total = invoice.Total,
                VatAmount = invoice.VatAmount,
                ShipperId = (invoice.Shipper==null)?0:invoice.Shipper.Id, //Ako dobijemo Shipper objekat kao null, stavimo nulu
                CustomerId = invoice.Customer.Id,
                AgentId = invoice.Agent.Id,
                Shipper = (invoice.Shipper==null)?"":invoice.Shipper.Name,
                Customer = invoice.Customer.Name,
                Agent = invoice.Agent.Name,              
                Shipping = invoice.Shipping
            };
        }

        public Invoice Create(InvoiceModel model)
        {
          
            return new Invoice()
            { 
                Id =model.Id,
                InvoiceNo = model.InvoiceNo,
                Date = model.Date,
                ShippedOn = model.ShippedOn,
                Status = (Status)model.Status,
                Vat = model.Vat,
                Shipping = model.Shipping,
                Customer = _unitOfWork.Customers.Get(model.CustomerId),
                Agent = _unitOfWork.Agents.Get(model.AgentId),
                Shipper = _unitOfWork.Shippers.Get(model.ShipperId)
            };
        }

        public TokenModel Create(AuthToken authToken, CurrentUserModel user)
        {
            return new Models.TokenModel()
            {
                Token = authToken.Token,
                Expiration = authToken.Expiration,
                CurrentUser = user
            };
        }
        
    }
}

