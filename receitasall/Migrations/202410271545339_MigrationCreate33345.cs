namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationCreate33345 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Recipes", "Author_ID", "dbo.Authors");
            DropIndex("dbo.Recipes", new[] { "Author_ID" });
            RenameColumn(table: "dbo.Recipes", name: "Author_ID", newName: "AuthorId");
            AlterColumn("dbo.Recipes", "AuthorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Recipes", "AuthorId");
            AddForeignKey("dbo.Recipes", "AuthorId", "dbo.Authors", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Recipes", new[] { "AuthorId" });
            AlterColumn("dbo.Recipes", "AuthorId", c => c.Int());
            RenameColumn(table: "dbo.Recipes", name: "AuthorId", newName: "Author_ID");
            CreateIndex("dbo.Recipes", "Author_ID");
            AddForeignKey("dbo.Recipes", "Author_ID", "dbo.Authors", "ID");
        }
    }
}
