using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Arysoft.WebsiteMVC.Basico.Models;

namespace Arysoft.WebsiteMVC.Basico.Areas.Admin.Models
{
    [NotMapped]
    public class PaginaIndexListViewModel
    {
        public Guid PaginaID { get; set; }
        public string Titulo { get; set; }
        public string EtiquetaMenu { get; set; }
        public string Resumen { get; set; }
        public BoolTipo Visible { get; set; }
        public string TargetUrl { get; set; }
        public PaginaTarget Target { get; set; }
        public BoolTipo TieneGaleria { get; set; }
        public int ContadorVisitas { get; set; }
        public IdiomaTipo Idioma { get; set; }
        public BoolTipo EsPrincipal { get; set; }
        public int MeGusta { get; set; }
        public PaginaStatus Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaActualizacion { get; set; }

        public string TituloPaginaPadre { get; set; }

        public int ContadorArchivos { get; set; }
        public int ContadorNotas { get; set; }

        public PaginaIndexListViewModel(Pagina pagina)
        {
            this.PaginaID = pagina.PaginaID;
            this.Titulo = pagina.Titulo;
            this.EtiquetaMenu = pagina.EtiquetaMenu;
            this.Resumen = pagina.Resumen;
            this.Visible = pagina.Visible;
            this.TargetUrl = pagina.TargetUrl;
            this.Target = pagina.Target;
            this.TieneGaleria = pagina.TieneGaleria;
            this.ContadorVisitas = pagina.ContadorVisitas;
            this.Idioma = pagina.Idioma;
            this.EsPrincipal = pagina.EsPrincipal;
            this.MeGusta = pagina.MeGusta;
            this.Status = pagina.Status;
            this.FechaActualizacion = pagina.FechaActualizacion;

            this.TituloPaginaPadre = pagina.PaginaPadre != null ? pagina.PaginaPadre.Titulo : string.Empty;

            this.ContadorArchivos = pagina.Archivos != null ? pagina.Archivos.Count : 0;
            this.ContadorNotas = pagina.Notas != null ? pagina.Notas.Count : 0;
        }
    } // PaginaIndexListViewModel
}