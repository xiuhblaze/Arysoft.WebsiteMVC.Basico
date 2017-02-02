namespace Arysoft.WebsiteMVC.Basico.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablaOpciones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Opciones",
                c => new
                    {
                        OpcionID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 50),
                        Valor = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.OpcionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Opciones");
        }
    }
}
