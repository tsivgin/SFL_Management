namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class announcement2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Announcement", "ClassId", c => c.Int(nullable: false));
            CreateIndex("dbo.Announcement", "ClassId");
            AddForeignKey("dbo.Announcement", "ClassId", "dbo.Class", "ClassId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Announcement", "ClassId", "dbo.Class");
            DropIndex("dbo.Announcement", new[] { "ClassId" });
            DropColumn("dbo.Announcement", "ClassId");
        }
    }
}
