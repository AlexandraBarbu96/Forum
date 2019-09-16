namespace Forum.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AdminId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.AspNetUsers", t => t.AdminId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        CreatorId = c.String(maxLength: 128),
                        CategoryId = c.Int(nullable: false),
                        CreateData = c.DateTime(nullable: false),
                        IsEdited = c.Boolean(nullable: false),
                        EditData = c.DateTime(nullable: false),
                        EditorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SubjectId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.AspNetUsers", t => t.EditorId)
                .Index(t => t.CreatorId)
                .Index(t => t.CategoryId)
                .Index(t => t.EditorId);
            
            CreateTable(
                "dbo.Commentaries",
                c => new
                    {
                        CommentaryId = c.Int(nullable: false, identity: true),
                        Reply = c.String(nullable: false),
                        UserId = c.String(maxLength: 128),
                        SubjectId = c.Int(nullable: false),
                        CreateData = c.DateTime(nullable: false),
                        IsEdited = c.Boolean(nullable: false),
                        EditData = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentaryId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SubjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subjects", "EditorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subjects", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Commentaries", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Commentaries", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "AdminId", "dbo.AspNetUsers");
            DropIndex("dbo.Commentaries", new[] { "SubjectId" });
            DropIndex("dbo.Commentaries", new[] { "UserId" });
            DropIndex("dbo.Subjects", new[] { "EditorId" });
            DropIndex("dbo.Subjects", new[] { "CategoryId" });
            DropIndex("dbo.Subjects", new[] { "CreatorId" });
            DropIndex("dbo.Categories", new[] { "AdminId" });
            DropTable("dbo.Commentaries");
            DropTable("dbo.Subjects");
            DropTable("dbo.Categories");
        }
    }
}
