namespace DailyMart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class domainclasseschanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "Category_Id" });
            RenameColumn(table: "dbo.Products", name: "Category_Id", newName: "CategoryId");
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.Orders", "Status", c => c.String());
            AddColumn("dbo.Products", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Products", "UpdatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "CategoryId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            DropColumn("dbo.Orders", "OrderStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "OrderStatus", c => c.String());
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.OrderItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.OrderItems", new[] { "ProductId" });
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            AlterColumn("dbo.Products", "CategoryId", c => c.Int());
            DropColumn("dbo.Products", "UpdatedOn");
            DropColumn("dbo.Products", "CreatedOn");
            DropColumn("dbo.Orders", "Status");
            DropTable("dbo.OrderItems");
            RenameColumn(table: "dbo.Products", name: "CategoryId", newName: "Category_Id");
            CreateIndex("dbo.Products", "Category_Id");
            AddForeignKey("dbo.Products", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
