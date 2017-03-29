namespace Billing.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class token : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Invoices", new[] { "Agent_Id" });
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            DropIndex("dbo.Invoices", new[] { "Shipper_Id" });
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

            AlterColumn("dbo.Invoices", "ShippedOn", c => c.DateTime());
            AlterColumn("dbo.Invoices", "Agent_Id", c => c.Int());
            AlterColumn("dbo.Invoices", "Customer_Id", c => c.Int());
            AlterColumn("dbo.Invoices", "Shipper_Id", c => c.Int());
            CreateIndex("dbo.Invoices", "Agent_Id");
            CreateIndex("dbo.Invoices", "Customer_Id");
            CreateIndex("dbo.Invoices", "Shipper_Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Events", "Invoice_Id", "dbo.Invoices");
            DropIndex("dbo.Events", new[] { "Invoice_Id" });
            DropIndex("dbo.Invoices", new[] { "Shipper_Id" });
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            DropIndex("dbo.Invoices", new[] { "Agent_Id" });
            AlterColumn("dbo.Invoices", "Shipper_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Invoices", "Customer_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Invoices", "Agent_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Invoices", "ShippedOn", c => c.DateTime(nullable: false));
            DropTable("dbo.Events");
            CreateIndex("dbo.Invoices", "Shipper_Id");
            CreateIndex("dbo.Invoices", "Customer_Id");
            CreateIndex("dbo.Invoices", "Agent_Id");
        }
    }
}
