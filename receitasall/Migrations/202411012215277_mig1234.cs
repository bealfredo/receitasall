namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1234 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cookbooks", "DateAdded", c => c.DateTime(nullable: false));
            AddColumn("dbo.Cookbooks", "DateUpdated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Cookbooks", "AccentColor", c => c.String());
            AddColumn("dbo.Recipes", "DateAdded", c => c.DateTime(nullable: false));
            AddColumn("dbo.Recipes", "DateUpdated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Recipes", "AccentColor", c => c.String());
            AlterColumn("dbo.Cookbooks", "Title", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Cookbooks", "Description", c => c.String(maxLength: 200));
            AlterColumn("dbo.Recipes", "Title", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Recipes", "Description", c => c.String(maxLength: 200));
            AlterColumn("dbo.Recipes", "Rendimento", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recipes", "Rendimento", c => c.String());
            AlterColumn("dbo.Recipes", "Description", c => c.String());
            AlterColumn("dbo.Recipes", "Title", c => c.String());
            AlterColumn("dbo.Cookbooks", "Description", c => c.String());
            AlterColumn("dbo.Cookbooks", "Title", c => c.String());
            DropColumn("dbo.Recipes", "AccentColor");
            DropColumn("dbo.Recipes", "DateUpdated");
            DropColumn("dbo.Recipes", "DateAdded");
            DropColumn("dbo.Cookbooks", "AccentColor");
            DropColumn("dbo.Cookbooks", "DateUpdated");
            DropColumn("dbo.Cookbooks", "DateAdded");
        }
    }
}
