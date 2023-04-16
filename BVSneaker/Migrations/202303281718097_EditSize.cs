namespace BVSneaker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_ProductStock", "Size", c => c.Int(nullable: false));
            DropColumn("dbo.tb_ProductStock", "Volume");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_ProductStock", "Volume", c => c.Int(nullable: false));
            DropColumn("dbo.tb_ProductStock", "Size");
        }
    }
}
