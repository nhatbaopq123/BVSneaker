namespace BVSneaker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCategoryAlias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Category", "Alias", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Category", "Alias");
        }
    }
}
