namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration77 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecipeCookbooks", "DateAdded", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecipeCookbooks", "DateAdded");
        }
    }
}
