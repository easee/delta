namespace Billing.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init_mig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Username = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceNo = c.String(),
                        Date = c.DateTime(nullable: false),
                        ShippedOn = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Vat = c.Double(nullable: false),
                        Shipping = c.Double(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Agent_Id = c.Int(),
                        Customer_Id = c.Int(),
                        Shipper_Id = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agents", t => t.Agent_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Shippers", t => t.Shipper_Id)
                .Index(t => t.Agent_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Shipper_Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Town_Id = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Towns", t => t.Town_Id)
                .Index(t => t.Town_Id);
            
            CreateTable(
                "dbo.Towns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Zip = c.String(),
                        Name = c.String(),
                        Region = c.Int(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Shippers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Town_Id = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Towns", t => t.Town_Id)
                .Index(t => t.Town_Id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Town_Id = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Towns", t => t.Town_Id)
                .Index(t => t.Town_Id);
            
            CreateTable(
                "dbo.Procurements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Document = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Product_Id = c.Int(nullable: false),
                        Supplier_Id = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .ForeignKey("dbo.Suppliers", t => t.Supplier_Id)
                .Index(t => t.Product_Id)
                .Index(t => t.Supplier_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Unit = c.String(),
                        Price = c.Double(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Category_Id = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Invoice_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Invoice_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Input = c.Double(nullable: false),
                        Output = c.Double(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        Invoice_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
                .Index(t => t.Invoice_Id);
            
            CreateTable(
                "dbo.ApiUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Secret = c.String(),
                        AppId = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuthTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        Expiration = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        ApiUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiUsers", t => t.ApiUser_Id)
                .Index(t => t.ApiUser_Id);
            
            CreateTable(
                "dbo.TownAgents",
                c => new
                    {
                        Town_Id = c.Int(nullable: false),
                        Agent_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Town_Id, t.Agent_Id })
                .ForeignKey("dbo.Towns", t => t.Town_Id)
                .ForeignKey("dbo.Agents", t => t.Agent_Id)
                .Index(t => t.Town_Id)
                .Index(t => t.Agent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthTokens", "ApiUser_Id", "dbo.ApiUsers");
            DropForeignKey("dbo.Events", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Customers", "Town_Id", "dbo.Towns");
            DropForeignKey("dbo.Suppliers", "Town_Id", "dbo.Towns");
            DropForeignKey("dbo.Procurements", "Supplier_Id", "dbo.Suppliers");
            DropForeignKey("dbo.Procurements", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Stocks", "Id", "dbo.Products");
            DropForeignKey("dbo.Items", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Items", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Shippers", "Town_Id", "dbo.Towns");
            DropForeignKey("dbo.Invoices", "Shipper_Id", "dbo.Shippers");
            DropForeignKey("dbo.TownAgents", "Agent_Id", "dbo.Agents");
            DropForeignKey("dbo.TownAgents", "Town_Id", "dbo.Towns");
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Invoices", "Agent_Id", "dbo.Agents");
            DropIndex("dbo.TownAgents", new[] { "Agent_Id" });
            DropIndex("dbo.TownAgents", new[] { "Town_Id" });
            DropIndex("dbo.AuthTokens", new[] { "ApiUser_Id" });
            DropIndex("dbo.Events", new[] { "Invoice_Id" });
            DropIndex("dbo.Stocks", new[] { "Id" });
            DropIndex("dbo.Items", new[] { "Product_Id" });
            DropIndex("dbo.Items", new[] { "Invoice_Id" });
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropIndex("dbo.Procurements", new[] { "Supplier_Id" });
            DropIndex("dbo.Procurements", new[] { "Product_Id" });
            DropIndex("dbo.Suppliers", new[] { "Town_Id" });
            DropIndex("dbo.Shippers", new[] { "Town_Id" });
            DropIndex("dbo.Customers", new[] { "Town_Id" });
            DropIndex("dbo.Invoices", new[] { "Shipper_Id" });
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            DropIndex("dbo.Invoices", new[] { "Agent_Id" });
            DropTable("dbo.TownAgents");
            DropTable("dbo.AuthTokens");
            DropTable("dbo.ApiUsers");
            DropTable("dbo.Events");
            DropTable("dbo.Stocks");
            DropTable("dbo.Items");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.Procurements");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Shippers");
            DropTable("dbo.Towns");
            DropTable("dbo.Customers");
            DropTable("dbo.Invoices");
            DropTable("dbo.Agents");
        }
    }
}
