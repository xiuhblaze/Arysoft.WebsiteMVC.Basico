using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arysoft.WebsiteMVC.Basico.Models
{
    [Table("Noticias")]
    public class Noticia
    {
        public Guid NoticiaID { get; set; }

        [Display(Name = "Título"), StringLength(200)]
        [Required(ErrorMessage = "Es necesario el título de la noticia")]
        public string Titulo { get; set; }

        [StringLength(1000)]
        public string Resumen { get; set; }

        [Display(Name = "Publicación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaPublicacion { get; set; }

        [Required(ErrorMessage = "Es necesario el contenido de la noticia")]
        public string Contenido { get; set; }

        [StringLength(150)]
        public string Autor { get; set; }
        
        [StringLength(255)]
        [Display(Name = "Imagen principal")]
        public string ImagenPrincipal { get; set; }

        public NoticiaEstatus Estatus { get; set; }

        // VIRTUAL

        [ForeignKey("PropietarioID")]
        public virtual ICollection<Archivo> Archivos { get; set; }
    }

    public enum NoticiaEstatus
    {
        [Display(Name = "(seleccionar estatus)")]
        Ninguno,    // Noticia temporal, aun no ha sido guardada
        Nueva,      // Noticia nueva, aun no es visible al público
        Publicada,  // Noticia publicada, la puede ver el público
        Oculta,     // No visible
        Eliminada   // Marcada como eliminada, solo un administrador la puede eliminar realmente o volverla a habilitar
    }
}