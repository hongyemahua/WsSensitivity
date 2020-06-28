namespace WsSensitivity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        sex = c.String(),
                        role = c.String(),
                        phone = c.Long(nullable: false),
                        email = c.String(),
                        pass = c.String(),
                        state = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Admins");
        }
    }
}
