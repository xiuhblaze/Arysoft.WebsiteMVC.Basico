namespace Arysoft.WebsiteMVC.Basico.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreacionInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archivos",
                c => new
                    {
                        ArchivoID = c.Guid(nullable: false),
                        PropietarioID = c.Guid(nullable: false),
                        Nombre = c.String(maxLength: 255),
                        Descripcion = c.String(maxLength: 1000),
                        Etiquetas = c.String(maxLength: 200),
                        FechaAlta = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ArchivoID)
                .ForeignKey("dbo.Noticias", t => t.PropietarioID, cascadeDelete: true)
                .ForeignKey("dbo.Paginas", t => t.PropietarioID, cascadeDelete: true)
                .Index(t => t.PropietarioID);
            
            CreateTable(
                "dbo.Noticias",
                c => new
                    {
                        NoticiaID = c.Guid(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 200),
                        Resumen = c.String(maxLength: 1000),
                        FechaPublicacion = c.DateTime(nullable: false),
                        Contenido = c.String(nullable: false),
                        Autor = c.String(maxLength: 150),
                        ImagenPrincipal = c.String(maxLength: 255),
                        Estatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NoticiaID);
            
            CreateTable(
                "dbo.Paginas",
                c => new
                    {
                        PaginaID = c.Guid(nullable: false),
                        PaginaPadreID = c.Guid(),
                        Titulo = c.String(nullable: false, maxLength: 150),
                        EtiquetaMenu = c.String(nullable: false, maxLength: 30),
                        Resumen = c.String(maxLength: 1000),
                        Contenido = c.String(),
                        TargetUrl = c.String(maxLength: 255),
                        Estatus = c.Int(nullable: false),
                        Target = c.Int(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaActualizacion = c.DateTime(nullable: false),
                        ContadorVisitas = c.Int(nullable: false),
                        HTMLHeadScript = c.String(),
                        HTMLBottomScript = c.String(),
                        Lenguaje = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PaginaID)
                .ForeignKey("dbo.Paginas", t => t.PaginaPadreID)
                .Index(t => t.PaginaPadreID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Paginas", "PaginaPadreID", "dbo.Paginas");
            DropForeignKey("dbo.Archivos", "PropietarioID", "dbo.Paginas");
            DropForeignKey("dbo.Archivos", "PropietarioID", "dbo.Noticias");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Paginas", new[] { "PaginaPadreID" });
            DropIndex("dbo.Archivos", new[] { "PropietarioID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Paginas");
            DropTable("dbo.Noticias");
            DropTable("dbo.Archivos");
        }
    }
}
