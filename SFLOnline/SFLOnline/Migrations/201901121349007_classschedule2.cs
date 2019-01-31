namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class classschedule2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClassSchedule", "DayId", "dbo.Day");
            DropForeignKey("dbo.ClassSchedule", "SlotId", "dbo.Slot");
            DropIndex("dbo.ClassSchedule", new[] { "SlotId" });
            DropIndex("dbo.ClassSchedule", new[] { "DayId" });
            DropPrimaryKey("dbo.Day");
            DropPrimaryKey("dbo.Slot");
            AddColumn("dbo.Day", "DayId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Day", "DaysName", c => c.String());
            AddColumn("dbo.Slot", "SlotId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Slot", "SlotName", c => c.String());
            AlterColumn("dbo.ClassSchedule", "SlotId", c => c.Int(nullable: false));
            AlterColumn("dbo.ClassSchedule", "DayId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Day", "DayId");
            AddPrimaryKey("dbo.Slot", "SlotId");
            CreateIndex("dbo.ClassSchedule", "SlotId");
            CreateIndex("dbo.ClassSchedule", "DayId");
            AddForeignKey("dbo.ClassSchedule", "DayId", "dbo.Day", "DayId", cascadeDelete: true);
            AddForeignKey("dbo.ClassSchedule", "SlotId", "dbo.Slot", "SlotId", cascadeDelete: true);
            DropColumn("dbo.Day", "Days");
            DropColumn("dbo.Slot", "Slots");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Slot", "Slots", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Day", "Days", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.ClassSchedule", "SlotId", "dbo.Slot");
            DropForeignKey("dbo.ClassSchedule", "DayId", "dbo.Day");
            DropIndex("dbo.ClassSchedule", new[] { "DayId" });
            DropIndex("dbo.ClassSchedule", new[] { "SlotId" });
            DropPrimaryKey("dbo.Slot");
            DropPrimaryKey("dbo.Day");
            AlterColumn("dbo.ClassSchedule", "DayId", c => c.String(maxLength: 128));
            AlterColumn("dbo.ClassSchedule", "SlotId", c => c.String(maxLength: 128));
            DropColumn("dbo.Slot", "SlotName");
            DropColumn("dbo.Slot", "SlotId");
            DropColumn("dbo.Day", "DaysName");
            DropColumn("dbo.Day", "DayId");
            AddPrimaryKey("dbo.Slot", "Slots");
            AddPrimaryKey("dbo.Day", "Days");
            CreateIndex("dbo.ClassSchedule", "DayId");
            CreateIndex("dbo.ClassSchedule", "SlotId");
            AddForeignKey("dbo.ClassSchedule", "SlotId", "dbo.Slot", "Slots");
            AddForeignKey("dbo.ClassSchedule", "DayId", "dbo.Day", "Days");
        }
    }
}
