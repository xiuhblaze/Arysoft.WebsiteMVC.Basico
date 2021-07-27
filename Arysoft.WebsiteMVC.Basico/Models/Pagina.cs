using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arysoft.WebsiteMVC.Basico.Models
{
    [Table("Paginas")]
    public class Pagina
    {
        public Guid PaginaID { get; set; }

        [Display(Name = "Página padre"), DisplayFormat(NullDisplayText = "(es página padre)")]
        public Guid? PaginaPadreID { get; set; }

        [Display(Name = "Título"), StringLength(150)]
        [Required(ErrorMessage = "Es necesario el título de la página")]
        public string Titulo { get; set; }

        [Display(Name = "Etiqueta para menú"), StringLength(30)]
        [Required(ErrorMessage = "Falta indicar la etiqueta para el menú")]
        public string EtiquetaMenu { get; set; }

        [StringLength(1000)]
        public string Resumen { get; set; }
                
        public string HtmlContent { get; set; }

        public BoolTipo Visible { get; set; }

        [Display(Name = "Dirección URL", Description = "Direccion url a la cual el enlace va a saltar."), StringLength(255)]
        public string TargetUrl { get; set; }

        public PaginaTarget Target { get; set; }

        public BoolTipo TieneGaleria { get; set; }

        [Display(Name = "Visitas")]
        public int ContadorVisitas { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Conteo desde")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaContador { get; set; }

        public IdiomaTipo Idioma { get; set; }

        public BoolTipo EsPrincipal { get; set; }

        [Display(Name = "Script encabezado")]
        public string HTMLHeadScript { get; set; }

        [Display(Name = "Script final")]
        public string HTMLFooterScript { get; set; }

        public int MeGusta { get; set; }

        public PaginaEstatus Status { get; set; }
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Creación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Actualización")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaActualizacion { get; set; }

        public string UsuarioActualizacion { get; set; }

        // VIRTUAL

        [ForeignKey("PaginaPadreID")]
        public virtual Pagina PaginaPadre { get; set; }

        [ForeignKey("PaginaPadreID")]
        public virtual ICollection<Pagina> PaginasHijo { get; set; }

        [ForeignKey("PropietarioID")]
        public virtual ICollection<Archivo> Archivos { get; set; }
    } // Pagina

    public enum PaginaEstatus
    {
        Ninguno,    // Página temporal, aun no ha sido guardada
        Nueva,      // Página nueva, aun no es visible al público
        Publicada,  // La página ha sido publicada y es visible al público
        Oculta,     // Oculta al público
        Eliminada   // Página eliminada, solo un administrador puede habilitarla nuevamente
    }

    public enum PaginaTarget
    {
        [Display(Name = "(seleccionar target)")]
        Ninguno,
        Blank,
        Parent,
        Self,
        Top
    }
}