namespace BVSneaker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Advertisement",
                c => new
                    {
                        Adv_Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false),
                        Image = c.String(nullable: false),
                        Alias = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        ModDate = c.DateTime(nullable: false),
                        ModBy = c.String(),
                    })
                .PrimaryKey(t => t.Adv_Id);
            
            CreateTable(
                "dbo.tb_Blog",
                c => new
                    {
                        Blog_Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        CoverImage = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        ModDate = c.DateTime(nullable: false),
                        ModBy = c.String(),
                    })
                .PrimaryKey(t => t.Blog_Id);
            
            CreateTable(
                "dbo.tb_Brand",
                c => new
                    {
                        Brand_Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Image = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Address = c.String(),
                        Alias = c.String(),
                    })
                .PrimaryKey(t => t.Brand_Id);
            
            CreateTable(
                "dbo.tb_Product",
                c => new
                    {
                        Product_Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Rate = c.Single(nullable: false),
                        Description = c.String(nullable: false),
                        ViewCount = c.Int(nullable: false),
                        isHome = c.Boolean(nullable: false),
                        isBestSeller = c.Boolean(nullable: false),
                        isFeature = c.Boolean(nullable: false),
                        isHot = c.Boolean(nullable: false),
                        Brand_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                        Topic_Id = c.Int(nullable: false),
                        Alias = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        ModDate = c.DateTime(nullable: false),
                        ModBy = c.String(),
                    })
                .PrimaryKey(t => t.Product_Id)
                .ForeignKey("dbo.tb_Brand", t => t.Brand_Id, cascadeDelete: true)
                .ForeignKey("dbo.tb_Category", t => t.Category_Id, cascadeDelete: true)
                .ForeignKey("dbo.tb_Topic", t => t.Topic_Id, cascadeDelete: true)
                .Index(t => t.Brand_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.Topic_Id);
            
            CreateTable(
                "dbo.tb_Category",
                c => new
                    {
                        Category_Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        CreateDate = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        ModDate = c.DateTime(nullable: false),
                        ModBy = c.String(),
                    })
                .PrimaryKey(t => t.Category_Id);
            
            CreateTable(
                "dbo.tb_OrderDetail",
                c => new
                    {
                        OrderDetail_Id = c.Int(nullable: false, identity: true),
                        Order_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                        Quanity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderDetail_Id)
                .ForeignKey("dbo.tb_Order", t => t.Order_Id, cascadeDelete: true)
                .ForeignKey("dbo.tb_Product", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Order_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.tb_Order",
                c => new
                    {
                        Order_Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(),
                        IsDelivered = c.Boolean(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        CustomerName = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PaymentMethod = c.String(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreateDate = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        ModDate = c.DateTime(nullable: false),
                        ModBy = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Order_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.tb_ProductImage",
                c => new
                    {
                        Image_Id = c.Int(nullable: false, identity: true),
                        Product_Id = c.Int(nullable: false),
                        Image = c.String(),
                        isDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Image_Id)
                .ForeignKey("dbo.tb_Product", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.tb_ProductStock",
                c => new
                    {
                        Stock_Id = c.Int(nullable: false, identity: true),
                        Product_Id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Volume = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Stock_Id)
                .ForeignKey("dbo.tb_Product", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.tb_Topic",
                c => new
                    {
                        Topic_Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        DetailTitle = c.String(nullable: false, maxLength: 150),
                        Description = c.String(),
                        Image = c.String(),
                        Alias = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        ModDate = c.DateTime(nullable: false),
                        ModBy = c.String(),
                    })
                .PrimaryKey(t => t.Topic_Id);
            
            CreateTable(
                "dbo.tb_Menu",
                c => new
                    {
                        Menu_Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        Description = c.String(maxLength: 4000),
                        Position = c.Int(nullable: false),
                        Alias = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        CreateBy = c.String(),
                        ModDate = c.DateTime(nullable: false),
                        ModBy = c.String(),
                    })
                .PrimaryKey(t => t.Menu_Id);
            
            CreateTable(
                "dbo.tb_Statistic",
                c => new
                    {
                        Stat_Id = c.Int(nullable: false, identity: true),
                        TimeStat = c.DateTime(nullable: false),
                        NumberOfVisits = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Stat_Id);
            
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Order", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.tb_Product", "Topic_Id", "dbo.tb_Topic");
            DropForeignKey("dbo.tb_ProductStock", "Product_Id", "dbo.tb_Product");
            DropForeignKey("dbo.tb_ProductImage", "Product_Id", "dbo.tb_Product");
            DropForeignKey("dbo.tb_OrderDetail", "Product_Id", "dbo.tb_Product");
            DropForeignKey("dbo.tb_OrderDetail", "Order_Id", "dbo.tb_Order");
            DropForeignKey("dbo.tb_Product", "Category_Id", "dbo.tb_Category");
            DropForeignKey("dbo.tb_Product", "Brand_Id", "dbo.tb_Brand");
            DropIndex("dbo.tb_ProductStock", new[] { "Product_Id" });
            DropIndex("dbo.tb_ProductImage", new[] { "Product_Id" });
            DropIndex("dbo.tb_Order", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.tb_OrderDetail", new[] { "Product_Id" });
            DropIndex("dbo.tb_OrderDetail", new[] { "Order_Id" });
            DropIndex("dbo.tb_Product", new[] { "Topic_Id" });
            DropIndex("dbo.tb_Product", new[] { "Category_Id" });
            DropIndex("dbo.tb_Product", new[] { "Brand_Id" });
            DropColumn("dbo.AspNetUsers", "Phone");
            DropColumn("dbo.AspNetUsers", "FullName");
            DropTable("dbo.tb_Statistic");
            DropTable("dbo.tb_Menu");
            DropTable("dbo.tb_Topic");
            DropTable("dbo.tb_ProductStock");
            DropTable("dbo.tb_ProductImage");
            DropTable("dbo.tb_Order");
            DropTable("dbo.tb_OrderDetail");
            DropTable("dbo.tb_Category");
            DropTable("dbo.tb_Product");
            DropTable("dbo.tb_Brand");
            DropTable("dbo.tb_Blog");
            DropTable("dbo.tb_Advertisement");
        }
    }
}
