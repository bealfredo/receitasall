namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationCreate33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "Difficulty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "Difficulty");
        }
    }
}
