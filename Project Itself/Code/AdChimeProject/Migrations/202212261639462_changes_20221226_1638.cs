namespace AdChimeProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes_20221226_1638 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.tRecipientSms", newName: "RecipientsLists");
            RenameTable(name: "dbo.tTemplateSms", newName: "TemplateSMS");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TemplateSMS", newName: "tTemplateSms");
            RenameTable(name: "dbo.RecipientsLists", newName: "tRecipientSms");
        }
    }
}
