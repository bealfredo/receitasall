namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NomeDaMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Value = c.String(),
                        RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.RecipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ingredients", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.Ingredients", new[] { "RecipeId" });
            DropTable("dbo.Ingredients");
            DropTable("dbo.Recipes");
        }
    }
}
