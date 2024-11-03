namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cookbooks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 200),
                        Image = c.String(),
                        IsPrivate = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        AccentColor = c.String(),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.RecipeCookbooks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RecipeId = c.Int(nullable: false),
                        CookbookId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cookbooks", t => t.CookbookId, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: false)
                .Index(t => t.RecipeId)
                .Index(t => t.CookbookId);
            
            CreateTable(
                "dbo.FavoriteRecipes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        RecipeId = c.Int(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: false)
                .Index(t => t.AuthorId)
                .Index(t => t.RecipeId);
            
            AddColumn("dbo.Recipes", "DateAdded", c => c.DateTime(nullable: false));
            AddColumn("dbo.Recipes", "DateUpdated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Recipes", "AccentColor", c => c.String());
            AddColumn("dbo.AspNetUsers", "Admin", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Authors", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Authors", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Authors", "Nacionality", c => c.String(maxLength: 50));
            AlterColumn("dbo.Authors", "Bibliography", c => c.String(maxLength: 500));
            AlterColumn("dbo.Authors", "Pseudonym", c => c.String(maxLength: 50));
            AlterColumn("dbo.Authors", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Recipes", "Title", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Recipes", "Description", c => c.String(maxLength: 200));
            AlterColumn("dbo.Recipes", "Rendimento", c => c.String(nullable: false));
            AlterColumn("dbo.Ingredients", "Value", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Steps", "Value", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavoriteRecipes", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.FavoriteRecipes", "AuthorId", "dbo.Authors");
            DropForeignKey("dbo.RecipeCookbooks", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.RecipeCookbooks", "CookbookId", "dbo.Cookbooks");
            DropForeignKey("dbo.Cookbooks", "AuthorId", "dbo.Authors");
            DropIndex("dbo.FavoriteRecipes", new[] { "RecipeId" });
            DropIndex("dbo.FavoriteRecipes", new[] { "AuthorId" });
            DropIndex("dbo.RecipeCookbooks", new[] { "CookbookId" });
            DropIndex("dbo.RecipeCookbooks", new[] { "RecipeId" });
            DropIndex("dbo.Cookbooks", new[] { "AuthorId" });
            AlterColumn("dbo.Steps", "Value", c => c.String());
            AlterColumn("dbo.Ingredients", "Value", c => c.String());
            AlterColumn("dbo.Recipes", "Rendimento", c => c.String());
            AlterColumn("dbo.Recipes", "Description", c => c.String());
            AlterColumn("dbo.Recipes", "Title", c => c.String());
            AlterColumn("dbo.Authors", "UserId", c => c.String());
            AlterColumn("dbo.Authors", "Pseudonym", c => c.String());
            AlterColumn("dbo.Authors", "Bibliography", c => c.String());
            AlterColumn("dbo.Authors", "Nacionality", c => c.String());
            AlterColumn("dbo.Authors", "LastName", c => c.String());
            AlterColumn("dbo.Authors", "FirstName", c => c.String());
            DropColumn("dbo.AspNetUsers", "Admin");
            DropColumn("dbo.Recipes", "AccentColor");
            DropColumn("dbo.Recipes", "DateUpdated");
            DropColumn("dbo.Recipes", "DateAdded");
            DropTable("dbo.FavoriteRecipes");
            DropTable("dbo.RecipeCookbooks");
            DropTable("dbo.Cookbooks");
        }
    }
}
