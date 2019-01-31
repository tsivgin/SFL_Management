namespace SFLOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gradepercenga : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GradePercentage", "ModuleId", c => c.Int(nullable: false));
            CreateIndex("dbo.GradePercentage", "ModuleId");
            AddForeignKey("dbo.GradePercentage", "ModuleId", "dbo.Module", "ModuleId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GradePercentage", "ModuleId", "dbo.Module");
            DropIndex("dbo.GradePercentage", new[] { "ModuleId" });
            DropColumn("dbo.GradePercentage", "ModuleId");
        }
    }
}
