namespace DailyMart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpaymenttypeinorder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Payment_Id", "dbo.Payments");
            DropIndex("dbo.Orders", new[] { "Payment_Id" });
            DropColumn("dbo.Orders", "Payment_Id");
            DropTable("dbo.Payments");
        }
        
        public override void Down()
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
            
            AddColumn("dbo.Orders", "Payment_Id", c => c.Int());
            CreateIndex("dbo.Orders", "Payment_Id");
            AddForeignKey("dbo.Orders", "Payment_Id", "dbo.Payments", "Id");
        }
    }
}
