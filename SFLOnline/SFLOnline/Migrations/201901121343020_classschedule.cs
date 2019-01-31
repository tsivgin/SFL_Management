namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class classschedule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassSchedule",
                c => new
                    {
                        ClassScheduleId = c.Int(nullable: false, identity: true),
                        EnrollmentInstructorId = c.Int(nullable: false),
                        SlotId = c.String(maxLength: 128),
                        DayId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ClassScheduleId)
                .ForeignKey("dbo.Day", t => t.DayId)
                .ForeignKey("dbo.EnrollmentInstructor", t => t.EnrollmentInstructorId, cascadeDelete: true)
                .ForeignKey("dbo.Slot", t => t.SlotId)
                .Index(t => t.EnrollmentInstructorId)
                .Index(t => t.SlotId)
                .Index(t => t.DayId);
            
            CreateTable(
                "dbo.Day",
                c => new
                    {
                        Days = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Days);
            
            CreateTable(
                "dbo.Slot",
                c => new
                    {
                        Slots = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Slots);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClassSchedule", "SlotId", "dbo.Slot");
            DropForeignKey("dbo.ClassSchedule", "EnrollmentInstructorId", "dbo.EnrollmentInstructor");
            DropForeignKey("dbo.ClassSchedule", "DayId", "dbo.Day");
            DropIndex("dbo.ClassSchedule", new[] { "DayId" });
            DropIndex("dbo.ClassSchedule", new[] { "SlotId" });
            DropIndex("dbo.ClassSchedule", new[] { "EnrollmentInstructorId" });
            DropTable("dbo.Slot");
            DropTable("dbo.Day");
            DropTable("dbo.ClassSchedule");
        }
    }
}
