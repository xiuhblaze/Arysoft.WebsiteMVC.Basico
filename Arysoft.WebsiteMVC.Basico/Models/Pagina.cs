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
                
        public string Contenido { get; set; }

        [Display(Name = "Dirección URL"), StringLength(255)]
        public string TargetUrl { get; set; }

        public bool Visible { get; set; }
        public PaginaTarget Target { get; set; }

        [Display(Name = "Alta"), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FechaAlta { get; set; }

        [Display(Name = "Actualización"), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime FechaActualizacion { get; set; }

        [Display(Name = "Visitas")]
        public int ContadorVisitas { get; set; }

        [Display(Name = "Script encabezado")]
        public string HTMLHeadScript { get; set; }

        [Display(Name = "Script final")]
        public string HTMLBottomScript { get; set; }

        public LenguajeTipo Lenguaje { get; set; }

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

    public enum LenguajeTipo
    {
        [Display(Name = "(seleccionar lenguaje)")]
        Ninguno,
        [Display(Name = "Español")]
        Espannol,
        [Display(Name = "Inglés")]
        Ingles
    }
}