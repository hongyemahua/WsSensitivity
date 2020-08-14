namespace WsSensitivity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UpDownExperiments", "udt_Power", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UpDownExperiments", "udt_Power", c => c.Int(nullable: false));
        }
    }
}
