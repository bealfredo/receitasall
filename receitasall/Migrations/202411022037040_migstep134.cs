namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migstep134 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ingredients", "Value", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Steps", "Value", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Steps", "Value", c => c.String());
            AlterColumn("dbo.Ingredients", "Value", c => c.String());
        }
    }
}
