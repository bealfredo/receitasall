namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationCreate333 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Nacionality = c.String(),
                        Image = c.String(),
                        Bibliography = c.String(),
                        Pseudonym = c.String(),
                        EmailContact = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Steps",
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
            
            AddColumn("dbo.Recipes", "IsPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Recipes", "PreparationTimeInMinutes", c => c.Int(nullable: false));
            AddColumn("dbo.Recipes", "Rendimento", c => c.String());
            AddColumn("dbo.Recipes", "Author_ID", c => c.Int());
            CreateIndex("dbo.Recipes", "Author_ID");
            AddForeignKey("dbo.Recipes", "Author_ID", "dbo.Authors", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "Author_ID", "dbo.Authors");
            DropForeignKey("dbo.Steps", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.Steps", new[] { "RecipeId" });
            DropIndex("dbo.Recipes", new[] { "Author_ID" });
            DropColumn("dbo.Recipes", "Author_ID");
            DropColumn("dbo.Recipes", "Rendimento");
            DropColumn("dbo.Recipes", "PreparationTimeInMinutes");
            DropColumn("dbo.Recipes", "IsPrivate");
            DropTable("dbo.Steps");
            DropTable("dbo.Authors");
        }
    }
}
