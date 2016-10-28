namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class password : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Password",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        _encryptedPassword = c.String(),
                        _salt = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.User", "Password_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.User", "Password_Id");
            AddForeignKey("dbo.User", "Password_Id", "dbo.Password", "Id");
            DropColumn("dbo.User", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Password", c => c.String(nullable: false));
            DropForeignKey("dbo.User", "Password_Id", "dbo.Password");
            DropIndex("dbo.User", new[] { "Password_Id" });
            DropColumn("dbo.User", "Password_Id");
            DropTable("dbo.Password");
        }
    }
}
