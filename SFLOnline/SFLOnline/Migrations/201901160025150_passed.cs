namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InformationPassed", "AttendanceLimit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InformationPassed", "AttendanceLimit");
        }
    }
}
