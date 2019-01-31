namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exit3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exit", "ForEnrollment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exit", "ForEnrollment");
        }
    }
}
