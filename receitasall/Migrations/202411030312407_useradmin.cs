namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useradmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Admin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Admin");
        }
    }
}
