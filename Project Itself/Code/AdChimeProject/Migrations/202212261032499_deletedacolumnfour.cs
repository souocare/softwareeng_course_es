namespace AdChimeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedacolumnfour : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tCampaign", "idrecipient");
        }

        public override void Down()
        {

            AddColumn("dbo.tCampaign", "idrecipient", c => c.Int());
        }
    }
}
