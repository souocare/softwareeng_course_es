namespace AdChimeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changessome : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.tUsers", newName: "AppUsers");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.AppUsers", newName: "tUsers");
        }
    }
}
