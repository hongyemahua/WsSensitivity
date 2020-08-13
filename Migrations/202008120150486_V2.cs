namespace WsSensitivity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UpDownExperiments", "udt_Power", c => c.Double(nullable: false));
            DropColumn("dbo.UpDownExperiments", "udt_Methodstate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UpDownExperiments", "udt_Methodstate", c => c.Int(nullable: false));
            AlterColumn("dbo.UpDownExperiments", "udt_Power", c => c.Int(nullable: false));
        }
    }
}
