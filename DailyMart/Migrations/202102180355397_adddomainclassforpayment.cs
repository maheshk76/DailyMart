namespace DailyMart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddomainclassforpayment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(nullable: false),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "PaymentId", c => c.Byte());
            AddColumn("dbo.Orders", "Payment_Id", c => c.Int());
            AlterColumn("dbo.Orders", "OrderStatus", c => c.String());
            CreateIndex("dbo.Orders", "Payment_Id");
            AddForeignKey("dbo.Orders", "Payment_Id", "dbo.Payments", "Id");
            DropColumn("dbo.Orders", "PaymentStatus");
            DropColumn("dbo.Orders", "PaymentType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "PaymentType", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "PaymentStatus", c => c.Int(nullable: false));
            DropForeignKey("dbo.Orders", "Payment_Id", "dbo.Payments");
            DropIndex("dbo.Orders", new[] { "Payment_Id" });
            AlterColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Payment_Id");
            DropColumn("dbo.Orders", "PaymentId");
            DropTable("dbo.Payments");
        }
    }
}
