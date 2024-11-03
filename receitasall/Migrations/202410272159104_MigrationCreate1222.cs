namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationCreate1222 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Cookbooks", "AuthorId");
            AddForeignKey("dbo.Cookbooks", "AuthorId", "dbo.Authors", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cookbooks", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Cookbooks", new[] { "AuthorId" });
        }
    }
}
