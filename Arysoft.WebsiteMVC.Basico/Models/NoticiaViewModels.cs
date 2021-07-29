using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arysoft.WebsiteMVC.Basico.Models
{
    public class NoticiaIndexListViewModel
    {
        public Guid NoticiaID { get; set; }

        [Display(Name = "Título")]
        public string Titulo { get; set; }

        public string Resumen { get; set; }

        public string Autor { get; set; }

        public BoolTipo TieneGaleria { get; set; }

        [Display(Name = "Visitas")]
        public int ContadorVisitas { get; set; }

        public IdiomaTipo Idioma { get; set; }

        [Display(Name = "Publicado")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaPublicacion { get; set; }

        public int MeGusta { get; set; }

        public NoticiaStatus Status { get; set; }

        public int ContadorArchivos { get; set; }

        public NoticiaIndexListViewModel(Noticia noticia)
        {
            this.NoticiaID = noticia.NoticiaID;
            this.Titulo = noticia.Titulo;
            this.Resumen = noticia.Resumen;
            this.Autor = noticia.Autor;
            this.TieneGaleria = noticia.TieneGaleria;
            this.ContadorVisitas = noticia.ContadorVisitas;
            this.Idioma = noticia.Idioma;
            this.FechaPublicacion = noticia.FechaPublicacion;
            this.MeGusta = noticia.MeGusta;
            this.Status = noticia.Status;
            this.ContadorArchivos = noticia.Archivos != null ? noticia.Archivos.Count : 0;
        }
    }
}