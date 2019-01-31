namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExitTrack",
                c => new
                    {
                        ExitTrackId = c.Int(nullable: false, identity: true),
                        ExitId = c.Int(nullable: false),
                        TrackId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExitTrackId)
                .ForeignKey("dbo.Exit", t => t.ExitId, cascadeDelete: true)
                .ForeignKey("dbo.Track", t => t.TrackId, cascadeDelete: true)
                .Index(t => t.ExitId)
                .Index(t => t.TrackId);
            
            CreateTable(
                "dbo.Exit",
                c => new
                    {
                        ExitId = c.Int(nullable: false, identity: true),
                        ExitName = c.String(),
                        ExitFresh = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExitId);
            
            CreateTable(
                "dbo.StudentExitGrade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(maxLength: 128),
                        ExitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exit", t => t.ExitId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.ExitId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExitTrack", "TrackId", "dbo.Track");
            DropForeignKey("dbo.ExitTrack", "ExitId", "dbo.Exit");
            DropForeignKey("dbo.StudentExitGrade", "StudentId", "dbo.Person");
            DropForeignKey("dbo.StudentExitGrade", "ExitId", "dbo.Exit");
            DropIndex("dbo.StudentExitGrade", new[] { "ExitId" });
            DropIndex("dbo.StudentExitGrade", new[] { "StudentId" });
            DropIndex("dbo.ExitTrack", new[] { "TrackId" });
            DropIndex("dbo.ExitTrack", new[] { "ExitId" });
            DropTable("dbo.StudentExitGrade");
            DropTable("dbo.Exit");
            DropTable("dbo.ExitTrack");
        }
    }
}
