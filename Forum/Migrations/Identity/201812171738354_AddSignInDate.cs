namespace Forum.Migrations.Identity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSignInDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SignInDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SignInDate");
        }
    }
}
