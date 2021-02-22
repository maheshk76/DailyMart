namespace DailyMart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedomainclasses : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "PaymentId");
            DropColumn("dbo.Products", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CategoryId", c => c.Byte());
            AddColumn("dbo.Orders", "PaymentId", c => c.Byte());
        }
    }
}
