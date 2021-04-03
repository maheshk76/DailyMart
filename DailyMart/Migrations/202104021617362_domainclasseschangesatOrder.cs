namespace DailyMart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class domainclasseschangesatOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderStatus", c => c.String());
            DropColumn("dbo.Orders", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Status", c => c.String());
            DropColumn("dbo.Orders", "OrderStatus");
        }
    }
}
