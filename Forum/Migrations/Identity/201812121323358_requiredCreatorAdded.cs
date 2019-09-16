namespace Forum.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredCreatorAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subjects", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Subjects", new[] { "CreatorId" });
            AlterColumn("dbo.Subjects", "CreatorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Subjects", "CreatorId");
            AddForeignKey("dbo.Subjects", "CreatorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subjects", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Subjects", new[] { "CreatorId" });
            AlterColumn("dbo.Subjects", "CreatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Subjects", "CreatorId");
            AddForeignKey("dbo.Subjects", "CreatorId", "dbo.AspNetUsers", "Id");
        }
    }
}
