namespace Arysoft.WebsiteMVC.Basico.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaginaNoticiaActualizado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Noticias", "HTMLContent", c => c.String(nullable: false));
            AddColumn("dbo.Noticias", "TieneGaleria", c => c.Int(nullable: false));
            AddColumn("dbo.Noticias", "ContadorVisitas", c => c.Int(nullable: false));
            AddColumn("dbo.Noticias", "Idioma", c => c.Int(nullable: false));
            AddColumn("dbo.Noticias", "ImagenArchivo", c => c.String(maxLength: 255));
            AddColumn("dbo.Noticias", "MeGusta", c => c.Int(nullable: false));
            AddColumn("dbo.Noticias", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Noticias", "FechaCreacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Noticias", "FechaActualizacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Noticias", "UsuarioActualizacion", c => c.String());
            AddColumn("dbo.Paginas", "HtmlContent", c => c.String());
            AddColumn("dbo.Paginas", "Visible", c => c.Int(nullable: false));
            AddColumn("dbo.Paginas", "TieneGaleria", c => c.Int(nullable: false));
            AddColumn("dbo.Paginas", "FechaContador", c => c.DateTime(nullable: false));
            AddColumn("dbo.Paginas", "Idioma", c => c.Int(nullable: false));
            AddColumn("dbo.Paginas", "EsPrincipal", c => c.Int(nullable: false));
            AddColumn("dbo.Paginas", "HTMLFooterScript", c => c.String());
            AddColumn("dbo.Paginas", "MeGusta", c => c.Int(nullable: false));
            AddColumn("dbo.Paginas", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Paginas", "FechaCreacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Paginas", "UsuarioActualizacion", c => c.String());
            DropColumn("dbo.Noticias", "Contenido");
            DropColumn("dbo.Noticias", "ImagenPrincipal");
            DropColumn("dbo.Noticias", "Estatus");
            DropColumn("dbo.Paginas", "Contenido");
            DropColumn("dbo.Paginas", "Estatus");
            DropColumn("dbo.Paginas", "FechaAlta");
            DropColumn("dbo.Paginas", "HTMLBottomScript");
            DropColumn("dbo.Paginas", "Lenguaje");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Paginas", "Lenguaje", c => c.Int(nullable: false));
            AddColumn("dbo.Paginas", "HTMLBottomScript", c => c.String());
            AddColumn("dbo.Paginas", "FechaAlta", c => c.DateTime(nullable: false));
            AddColumn("dbo.Paginas", "Estatus", c => c.Int(nullable: false));
            AddColumn("dbo.Paginas", "Contenido", c => c.String());
            AddColumn("dbo.Noticias", "Estatus", c => c.Int(nullable: false));
            AddColumn("dbo.Noticias", "ImagenPrincipal", c => c.String(maxLength: 255));
            AddColumn("dbo.Noticias", "Contenido", c => c.String(nullable: false));
            DropColumn("dbo.Paginas", "UsuarioActualizacion");
            DropColumn("dbo.Paginas", "FechaCreacion");
            DropColumn("dbo.Paginas", "Status");
            DropColumn("dbo.Paginas", "MeGusta");
            DropColumn("dbo.Paginas", "HTMLFooterScript");
            DropColumn("dbo.Paginas", "EsPrincipal");
            DropColumn("dbo.Paginas", "Idioma");
            DropColumn("dbo.Paginas", "FechaContador");
            DropColumn("dbo.Paginas", "TieneGaleria");
            DropColumn("dbo.Paginas", "Visible");
            DropColumn("dbo.Paginas", "HtmlContent");
            DropColumn("dbo.Noticias", "UsuarioActualizacion");
            DropColumn("dbo.Noticias", "FechaActualizacion");
            DropColumn("dbo.Noticias", "FechaCreacion");
            DropColumn("dbo.Noticias", "Status");
            DropColumn("dbo.Noticias", "MeGusta");
            DropColumn("dbo.Noticias", "ImagenArchivo");
            DropColumn("dbo.Noticias", "Idioma");
            DropColumn("dbo.Noticias", "ContadorVisitas");
            DropColumn("dbo.Noticias", "TieneGaleria");
            DropColumn("dbo.Noticias", "HTMLContent");
        }
    }
}
