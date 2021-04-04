namespace DailyMart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datetypechangeforcategory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "UpdateOn", c => c.DateTime());
            AlterColumn("dbo.Categories", "CreatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Categories", "UpdateOn", c => c.DateTime(nullable: false));
        }
    }
}
