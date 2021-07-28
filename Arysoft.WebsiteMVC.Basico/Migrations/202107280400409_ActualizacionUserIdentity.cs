namespace Arysoft.WebsiteMVC.Basico.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizacionUserIdentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Nombres", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "PrimerApellido", c => c.String(maxLength: 50));
            AddColumn("dbo.AspNetUsers", "SegundoApellido", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SegundoApellido");
            DropColumn("dbo.AspNetUsers", "PrimerApellido");
            DropColumn("dbo.AspNetUsers", "Nombres");
        }
    }
}
