namespace DailyMart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class descpropertyforcategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Description", c => c.String());
            AddColumn("dbo.Categories", "UpdateOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Categories", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "CreatedOn");
            DropColumn("dbo.Categories", "UpdateOn");
            DropColumn("dbo.Categories", "Description");
        }
    }
}
