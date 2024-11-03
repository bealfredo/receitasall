namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2234 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecipeCookbooks", "order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecipeCookbooks", "order");
        }
    }
}
