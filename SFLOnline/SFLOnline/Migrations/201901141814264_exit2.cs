namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exit2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExitTrack", "grade", c => c.Int(nullable: false));
            AddColumn("dbo.StudentExitGrade", "grade", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentExitGrade", "grade");
            DropColumn("dbo.ExitTrack", "grade");
        }
    }
}
