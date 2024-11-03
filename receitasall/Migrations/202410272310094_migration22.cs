namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration22 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RecipeCookbooks", "Accepted");
            DropColumn("dbo.RecipeCookbooks", "DateAdded");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RecipeCookbooks", "DateAdded", c => c.DateTime(nullable: false));
            AddColumn("dbo.RecipeCookbooks", "Accepted", c => c.Boolean(nullable: false));
        }
    }
}
