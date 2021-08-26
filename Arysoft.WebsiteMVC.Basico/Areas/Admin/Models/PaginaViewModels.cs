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

    [NotMapped]
    public class PaginaDetailsViewModel : Pagina 
    {
        /// <summary>
        /// Obtiene o establece un valor que se pasa a las vistas y determina si el origen del modelo
        /// es desde un metodo de consulta o para dar de baja el registro
        /// </summary>        
        public string Origen { get; set; }

        /// <summary>
        /// Obtiene o establece un valor qe se pasa a las vistas para determinar si la información
        /// se va a mostrar como solo lectura o para edición
        /// </summary>        
        public bool SoloLectura { get; set; }

        // CONSTRUCTORES

        public PaginaDetailsViewModel(Pagina pagina, string origen = "", bool? soloLectura = false)
        {
            this.PaginaID = pagina.PaginaID;
            this.PaginaPadreID = pagina.PaginaPadreID;
            this.Titulo = pagina.Titulo;
            this.IndiceMenu = pagina.IndiceMenu;
            this.EtiquetaMenu = pagina.EtiquetaMenu;
            this.Resumen = pagina.Resumen;
            this.HTMLContent = pagina.HTMLContent;
            this.Visible = pagina.Visible;            
            this.TargetUrl = pagina.TargetUrl;
            this.Target = pagina.Target;
            this.TieneGaleria = pagina.TieneGaleria;
            this.ContadorVisitas = pagina.ContadorVisitas;
            this.FechaContador = pagina.FechaContador;
            this.Idioma = pagina.Idioma;
            this.EsPrincipal = pagina.EsPrincipal;
            this.HTMLHeadScript = pagina.HTMLHeadScript;
            this.HTMLFooterScript = pagina.HTMLFooterScript;
            this.MeGusta = pagina.MeGusta;
            this.Status = pagina.Status;
            
            this.FechaCreacion = pagina.FechaCreacion;
            this.FechaActualizacion = pagina.FechaActualizacion;
            this.UsuarioActualizacion = pagina.UsuarioActualizacion;

            this.PaginaPadre = pagina.PaginaPadre;
            this.Archivos = pagina.Archivos.OrderBy(a => a.Nombre).ToList();
            this.Notas = pagina.Notas.OrderByDescending(n => n.FechaCreacion).ToList();

            this.Origen = origen;
            this.SoloLectura = soloLectura ?? false;
        } // PaginaDetailsViewModel :: Constructor

    } // PaginaDetailsViewModel
}