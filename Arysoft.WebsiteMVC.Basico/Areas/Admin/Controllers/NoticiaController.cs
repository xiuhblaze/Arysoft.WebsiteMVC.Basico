using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Arysoft.WebsiteMVC.Basico.Models;
using PagedList;

namespace Arysoft.WebsiteMVC.Basico.Areas.Admin.Controllers
{
    public class NoticiaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Admin/Noticia
        public ActionResult Index(string buscar, string filtro, string orden, string fechaRango, string estatus, int? pagina)
        {
            ViewBag.Orden = orden;
            ViewBag.OrdenFecha = string.IsNullOrEmpty(orden) ? "fecha" : "";
            ViewBag.OrdenTitulo = orden == "titulo" ? "titulo_desc" : "titulo";
            ViewBag.OrdenEstatus = orden == "estatus" ? "estatus_desc" : "estatus";

            if (buscar != null) { pagina = 1; }
            else { buscar = filtro ?? string.Empty; }

            ViewBag.Filtro = buscar;

            var noticias = from n in db.Noticias select n;

            if (!string.IsNullOrEmpty(buscar) 
                || (!string.IsNullOrEmpty(fechaRango) && fechaRango != "0")
                || !string.IsNullOrEmpty(estatus))
            {
                DateTime fechaInicio = DateTime.MinValue;
                DateTime fechaTermino = DateTime.MaxValue;

                if (!string.IsNullOrEmpty(fechaRango) && fechaRango != "0")
                {
                    DateTime fecha = DateTime.Today;

                    if (!string.IsNullOrEmpty(buscar) && isDate(buscar))
                    {
                        fecha = DateTime.Parse(buscar);
                        buscar = string.Empty;
                    }

                    switch (fechaRango)
                    {
                        case "1": // Día
                            fechaInicio = fecha;
                            fechaTermino = fecha.AddDays(1).AddSeconds(-1);
                            break;
                        case "2": // Semana
                            // Inicio de semana, obtenido de: http://joelabrahamsson.com/getting-the-first-day-in-a-week-with-c/ en los comentarios
                            fechaInicio = fecha.AddDays(-((fecha.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
                            fechaTermino = fechaInicio.AddDays(6);
                            break;
                        case "3": // Mes
                            fechaInicio = new DateTime(fecha.Year, fecha.Month, 1);
                            fechaTermino = new DateTime(fecha.AddMonths(1).Year, fecha.AddMonths(1).Month, 1).AddDays(-1);
                            break;
                        case "4": // Año
                            fechaInicio = new DateTime(fecha.Year, 1, 1);
                            fechaTermino = new DateTime(fecha.AddYears(1).Year, 1, 1).AddDays(-1);
                            break;
                    }

                    ViewBag.FechaRango = fechaRango;                    
                }
                else
                {
                    ViewBag.FechaRango = "0";
                } // Del filtro por fecha

                string[] palabrasBuscar = buscar.Split(new char[] { ' ', '.', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                bool hayPalabras = palabrasBuscar.Length != 0;

                noticias = noticias.Where(n =>
                    (
                        (!hayPalabras)
                        || (from p in palabrasBuscar where n.Titulo.Contains(p) select p).ToList().Count > 0
                        || (from p in palabrasBuscar where n.Resumen.Contains(p) select p).ToList().Count > 0
                        || (from p in palabrasBuscar where n.Contenido.Contains(p) select p).ToList().Count > 0
                        || (from p in palabrasBuscar where n.Autor.Contains(p) select p).ToList().Count > 0
                    )
                    && (estatus == "Ninguno" ? true : n.Estatus.ToString() == estatus)
                    && (fechaRango == "0" ? true : (n.FechaPublicacion >= fechaInicio && n.FechaPublicacion <= fechaTermino))
                );
            } // buscar

            ViewBag.Contar = (from n in noticias select n).Count();

            switch (orden)
            {
                case "fecha":
                    noticias = noticias.OrderBy(n => n.FechaPublicacion);
                    break;
                case "titulo":
                    noticias = noticias.OrderBy(n => n.Titulo);
                    break;
                case "titulo_desc":
                    noticias = noticias.OrderByDescending(n => n.Titulo);
                    break;
                case "estatus":
                    noticias = noticias.OrderBy(n => n.Estatus);
                    break;
                case "estatus_desc":
                    noticias = noticias.OrderByDescending(n => n.Estatus);
                    break;
                default:
                    noticias = noticias.OrderByDescending(n => n.FechaPublicacion);
                    break;
            }

            ViewBag.Crear = true; 
            ViewBag.Editar = true;
            ViewBag.Eliminar = true;

            ViewBag.Estatus = estatus;

            int numeroPagina = pagina ?? 1;
            int elementosPorPagina = 25;

            return View(noticias.ToPagedList(numeroPagina, elementosPorPagina));
        } // Index:GET

        // GET: Admin/Noticia/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = db.Noticias.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        // GET: Admin/Noticia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Noticia/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "NoticiaID,Titulo,Resumen,FechaPublicacion,Contenido,Autor,ImagenPrincipal,Estatus")] Noticia noticia, HttpPostedFileBase archivoImagenPrincipal)
        {
            string nombreArchivo = string.Empty;
            Guid newID = Guid.NewGuid();

            // Validando datos de entrada

            if (noticia.Estatus == NoticiaEstatus.Ninguno)
            {
                noticia.Estatus = NoticiaEstatus.Nueva;
            }

            // Validar archivo de la imagen principal
            try
            {
                if (archivoImagenPrincipal != null && archivoImagenPrincipal.ContentLength > 0)
                {
                    noticia.ImagenPrincipal = SubirArchivo(archivoImagenPrincipal, newID.ToString(), "imgPrincipal");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            if (ModelState.IsValid)
            {
                noticia.NoticiaID = newID;
                db.Noticias.Add(noticia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(noticia);
        }

        // GET: Admin/Noticia/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = db.Noticias.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        // POST: Admin/Noticia/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "NoticiaID,Titulo,Resumen,FechaPublicacion,Contenido,Autor,ImagenPrincipal,Estatus")] Noticia noticia, HttpPostedFileBase archivoImagenPrincipal)
        {

            // Validar archivo de la imagen principal
            try
            {
                if (archivoImagenPrincipal != null && archivoImagenPrincipal.ContentLength > 0)
                {
                    noticia.ImagenPrincipal = SubirArchivo(archivoImagenPrincipal, noticia.NoticiaID.ToString(), "imgPrincipal");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            if (ModelState.IsValid)
            {
                db.Entry(noticia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(noticia);
        }

        // GET: Admin/Noticia/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = db.Noticias.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        // POST: Admin/Noticia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Noticia noticia = db.Noticias.Find(id);
            db.Noticias.Remove(noticia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // METODOS LOCALES

        private string SubirArchivo(HttpPostedFileBase fileControl, string carpeta, string nombre)
        {
            var path = Path.Combine(Server.MapPath("~/Archivos/Noticias/"), carpeta);
            var extencion = Path.GetExtension(fileControl.FileName).ToLower();

            if (string.IsNullOrEmpty(nombre)) nombre = fileControl.FileName;
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

            var completePath = Path.Combine(path, nombre + extencion);
            fileControl.SaveAs(completePath);

            return nombre + extencion;
        } // SubirArchivo

        public static bool isDate(Object obj)
        {
            string fecha = obj.ToString();

            try
            {
                DateTime dt = DateTime.Parse(fecha);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue) return true;

                return false;
            }
            catch
            {
                return false;
            }
        } // isDate


    }
}
