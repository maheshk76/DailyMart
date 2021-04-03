namespace DailyMart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messagesadded1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        Email = c.String(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 30),
                        Text = c.String(nullable: false, maxLength: 300),
                        CreatedOn = c.DateTime(nullable: false),
                        isRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
