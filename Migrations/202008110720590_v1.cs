namespace WsSensitivity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {

            Sql(@"create view UpDownViews as select UpDownDataTables.Id as uddt_Id,UpDownGroups.Id as udg_Id,UpDownGroups.dudt_ExperimentId,UpDownGroups.dudt_Stepd,UpDownDataTables.dtup_Initialstimulus,UpDownDataTables.dtup_response,UpDownDataTables.dtup_Standardstimulus from UpDownGroups,UpDownDataTables where UpDownDataTables.dtup_DataTableId=UpDownGroups.Id");
            //CreateTable(
            //    "dbo.UpDownViews",
            //    c => new
            //        {
            //            uddt_Id = c.Int(nullable: false, identity: true),
            //            udg_Id = c.Int(nullable: false),
            //            dudt_ExperimentId = c.Int(nullable: false),
            //            dudt_Stepd = c.Double(nullable: false),
            //            dtup_Initialstimulus = c.Double(nullable: false),
            //            dtup_response = c.Int(nullable: false),
            //            dtup_Standardstimulus = c.Double(nullable: false),
            //        })
            //    .PrimaryKey(t => t.uddt_Id);

        }

        public override void Down()
        {
            DropTable("dbo.UpDownViews");
        }
    }
}
