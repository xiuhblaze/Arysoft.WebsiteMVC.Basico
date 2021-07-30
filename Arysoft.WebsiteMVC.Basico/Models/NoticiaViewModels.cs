using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arysoft.WebsiteMVC.Basico.Models
{
    [NotMapped]
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
    } // NoticiaIndexListViewModel

    [NotMapped]
    public class NoticiaDetailsViewModel : Noticia
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
        //public NoticiaDetailsViewModel()
        //{
        //     // AQUI ESTA EL PUTO CONSTRUCTOR SIN PARAMETROS
        //} // NoticiaDetailsViewModel

        public NoticiaDetailsViewModel(Noticia noticia, string origen = "", bool? soloLectura = false)
        {
            this.NoticiaID = noticia.NoticiaID;
            this.Titulo = noticia.Titulo;
            this.Resumen = noticia.Resumen;
            this.HTMLContent = noticia.HTMLContent;
            this.Autor = noticia.Autor;
            this.TieneGaleria = noticia.TieneGaleria;
            this.ContadorVisitas = noticia.ContadorVisitas;
            this.Idioma = noticia.Idioma;            
            this.FechaPublicacion = noticia.FechaPublicacion;
            this.ImagenArchivo = noticia.ImagenArchivo;
            this.MeGusta = noticia.MeGusta;
            this.Status = noticia.Status;

            this.FechaCreacion = noticia.FechaCreacion;
            this.FechaActualizacion = noticia.FechaActualizacion;
            this.UsuarioActualizacion = noticia.UsuarioActualizacion;

            this.Archivos = noticia.Archivos;

            this.Origen = origen;
            this.SoloLectura = soloLectura ?? false;
        } // NoticiaDetailsViewModel :: Constructor

    } // NoticiaDetailsViewModel
}