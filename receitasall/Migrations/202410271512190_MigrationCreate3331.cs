namespace receitasall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationCreate3331 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Authors", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Authors", "UserId", c => c.Int(nullable: false));
        }
    }
}
