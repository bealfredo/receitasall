namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrationdatanotation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cookbooks", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.Recipes", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.Ingredients", "Value", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Steps", "Value", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Steps", "Value", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Ingredients", "Value", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Recipes", "Description", c => c.String(maxLength: 200));
            AlterColumn("dbo.Cookbooks", "Description", c => c.String(maxLength: 200));
        }
    }
}
