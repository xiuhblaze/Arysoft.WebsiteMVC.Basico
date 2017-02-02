namespace Arysoft.WebsiteMVC.Basico.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosenArchivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Archivos", "Tags", c => c.String(maxLength: 200));
            AddColumn("dbo.Archivos", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Archivos", "FechaActualizacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Archivos", "UsuarioActualizacion", c => c.String(maxLength: 256));
            DropColumn("dbo.Archivos", "Etiquetas");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Archivos", "Etiquetas", c => c.String(maxLength: 200));
            DropColumn("dbo.Archivos", "UsuarioActualizacion");
            DropColumn("dbo.Archivos", "FechaActualizacion");
            DropColumn("dbo.Archivos", "Status");
            DropColumn("dbo.Archivos", "Tags");
        }
    }
}
