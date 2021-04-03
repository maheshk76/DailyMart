namespace DailyMart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wishlistuseridtypechanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Wishlists", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Wishlists", "UserId", c => c.Int(nullable: false));
        }
    }
}
