namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attendance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentAttendance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(maxLength: 128),
                        Attendance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.StudentId)
                .Index(t => t.StudentId);
            
            DropTable("dbo.gradeGet");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.gradeGet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Grade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.StudentAttendance", "StudentId", "dbo.Person");
            DropIndex("dbo.StudentAttendance", new[] { "StudentId" });
            DropTable("dbo.StudentAttendance");
        }
    }
}
