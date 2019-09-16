namespace Forum.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.AspNetUsers", "Photo", c => c.Binary());
            DropColumn("dbo.AspNetUsers", "UserPhoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserPhoto", c => c.Binary());
            DropForeignKey("dbo.Files", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Files", new[] { "User_Id" });
            DropColumn("dbo.AspNetUsers", "Photo");
            DropTable("dbo.Files");
        }
    }
}
