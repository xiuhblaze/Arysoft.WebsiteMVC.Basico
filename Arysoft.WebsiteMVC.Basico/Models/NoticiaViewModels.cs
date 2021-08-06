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
            this.Notas = noticia.Notas;

            this.Origen = origen;
            this.SoloLectura = soloLectura ?? false;
        } // NoticiaDetailsViewModel :: Constructor

    } // NoticiaDetailsViewModel

    public class NoticiaEditViewModel
    {
        public Guid NoticiaID { get; set; }

        [Display(Name = "Título"), StringLength(200)]
        [Required(ErrorMessage = "Es necesario el título de la noticia")]
        public string Titulo { get; set; }

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string __resumen { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Es necesario el contenido de la noticia")]
        public string __content { get; set; }

        [StringLength(150)]
        public string Autor { get; set; }

        // OPCIONES

        [Display(Name = "Tiene galeria")]
        public BoolTipo TieneGaleria { get; set; }

        public int ContadorVisitas { get; set; }

        public IdiomaTipo Idioma { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de publicación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaPublicacion { get; set; }

        [StringLength(255)]
        [Display(Name = "Imagen principal")]
        public string ImagenArchivo { get; set; } // PROBABLEMENTE SE GUARDE APARTE

        public int MeGusta { get; set; }

        public NoticiaStatus Status { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Creación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Actualización")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaActualizacion { get; set; }

        [Display(Name = "Actualizado por")]
        public string UsuarioActualizacion { get; set; }

        // RELACIONES

        public ICollection<Archivo> Archivos { get; set; }

        public ICollection<Nota> Notas { get; set; }

        // CONSTRUCTORES

        public NoticiaEditViewModel()
        { }

        public NoticiaEditViewModel(Noticia n)
        {
            this.NoticiaID = n.NoticiaID;
            this.Titulo = n.Titulo;
            this.__resumen = n.Resumen;
            this.__content = n.HTMLContent;
            this.Autor = n.Autor;

            this.TieneGaleria = n.TieneGaleria;
            this.ContadorVisitas = n.ContadorVisitas;
            this.Idioma = n.Idioma;
            this.FechaPublicacion = n.FechaPublicacion;
            this.ImagenArchivo = n.ImagenArchivo;
            this.MeGusta = n.MeGusta;
            this.Status = n.Status;

            this.FechaCreacion = n.FechaCreacion;
            this.FechaActualizacion = n.FechaActualizacion;
            this.UsuarioActualizacion = n.UsuarioActualizacion;

            this.Archivos = n.Archivos;
            this.Notas = n.Notas;
        } // NoticiaEditViewModel : Constructor

        // METODOS

        public Noticia ObtenerNoticia()
        {
            Noticia n = new Noticia
            {
                NoticiaID = this.NoticiaID,
                Titulo = this.Titulo,
                Resumen = this.__resumen,
                HTMLContent = this.__content,
                Autor = this.Autor,
                TieneGaleria = this.TieneGaleria,
                ContadorVisitas = this.ContadorVisitas,
                Idioma = this.Idioma,
                FechaPublicacion = this.FechaPublicacion,
                ImagenArchivo = this.ImagenArchivo,
                MeGusta = this.MeGusta,
                Status = this.Status,
                FechaCreacion = this.FechaCreacion
            };

            return n;
        } // ObtenerNoticia

    } // NoticiaEditViewModel
}