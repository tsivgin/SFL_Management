namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class announcement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Announcement",
                c => new
                    {
                        AnnouncementId = c.Int(nullable: false, identity: true),
                        WriterId = c.String(maxLength: 128),
                        AnnouncementName = c.String(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.AnnouncementId)
                .ForeignKey("dbo.Person", t => t.WriterId)
                .Index(t => t.WriterId);
            
            CreateTable(
                "dbo.InformationPassed",
                c => new
                    {
                        InformationPassedId = c.Int(nullable: false, identity: true),
                        ExitId = c.Int(nullable: false),
                        gradeAverage = c.Int(nullable: false),
                        passed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InformationPassedId)
                .ForeignKey("dbo.Exit", t => t.ExitId, cascadeDelete: true)
                .Index(t => t.ExitId);
            
            DropColumn("dbo.Exit", "ExitFresh");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Exit", "ExitFresh", c => c.Int(nullable: false));
            DropForeignKey("dbo.Announcement", "WriterId", "dbo.Person");
            DropForeignKey("dbo.InformationPassed", "ExitId", "dbo.Exit");
            DropIndex("dbo.InformationPassed", new[] { "ExitId" });
            DropIndex("dbo.Announcement", new[] { "WriterId" });
            DropTable("dbo.InformationPassed");
            DropTable("dbo.Announcement");
        }
    }
}
