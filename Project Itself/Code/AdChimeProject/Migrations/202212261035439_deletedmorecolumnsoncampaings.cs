namespace AdChimeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedmorecolumnsoncampaings : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tCampaign", "sendDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tCampaign", "sendDate", c => c.DateTime());
        }
    }
}
