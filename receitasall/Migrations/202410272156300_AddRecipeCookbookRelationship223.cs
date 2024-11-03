namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecipeCookbookRelationship223 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cookbooks", "AuthorId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cookbooks", "AuthorId");
        }
    }
}
