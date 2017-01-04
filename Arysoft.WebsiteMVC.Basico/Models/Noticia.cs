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

        [DataType(DataType.DateTime), Display(Name = "Publicación")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime FechaPublicacion { get; set; }

        public string Contenido { get; set; }

        [StringLength(150)]
        public string Autor { get; set; }
        
        [StringLength(255)]
        public string ImagenPrincipal { get; set; }

        public NoticiaEstatus Estatus { get; set; }

        // VIRTUAL

        [ForeignKey("PropietarioID")]
        public virtual ICollection<Archivo> Archivos { get; set; }
    }

    public enum NoticiaEstatus
    {
        Ninguno,
        Nueva,
        Publicada,
        Oculta,
        Eliminada
    }
}