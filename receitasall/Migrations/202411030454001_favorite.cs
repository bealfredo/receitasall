namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class favorite : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: false)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.RecipeId);
            
            AlterColumn("dbo.Authors", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Authors", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Authors", "Nacionality", c => c.String(maxLength: 50));
            AlterColumn("dbo.Authors", "Bibliography", c => c.String(maxLength: 500));
            AlterColumn("dbo.Authors", "Pseudonym", c => c.String(maxLength: 50));
            AlterColumn("dbo.Authors", "UserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavoriteRecipes", "RecipeId", "dbo.Recipes");
            DropForeignKey("dbo.FavoriteRecipes", "AuthorId", "dbo.Authors");
            DropIndex("dbo.FavoriteRecipes", new[] { "RecipeId" });
            DropIndex("dbo.FavoriteRecipes", new[] { "AuthorId" });
            AlterColumn("dbo.Authors", "UserId", c => c.String());
            AlterColumn("dbo.Authors", "Pseudonym", c => c.String());
            AlterColumn("dbo.Authors", "Bibliography", c => c.String());
            AlterColumn("dbo.Authors", "Nacionality", c => c.String());
            AlterColumn("dbo.Authors", "LastName", c => c.String());
            AlterColumn("dbo.Authors", "FirstName", c => c.String());
            DropTable("dbo.FavoriteRecipes");
        }
    }
}
