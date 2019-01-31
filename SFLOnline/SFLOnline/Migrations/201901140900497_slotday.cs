namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class slotday : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Day", "DayNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Slot", "SlotNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slot", "SlotNumber");
            DropColumn("dbo.Day", "DayNumber");
        }
    }
}
