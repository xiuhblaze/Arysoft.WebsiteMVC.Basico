using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Arysoft.WebsiteMVC.Basico.Models;
using Arysoft.WebsiteMVC.Basico.Areas.Admin.Models;
using PagedList;

namespace Arysoft.WebsiteMVC.Basico.Areas.Admin.Controllers
{
    public class NoticiasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Noticias
        public async Task<ActionResult> Index(string buscar, string filtro, string orden, string status, int? pagina)
        {
            NoticiaStatus myStatus = (NoticiaStatus)(string.IsNullOrEmpty(status) ? NoticiaStatus.Ninguno : Enum.Parse(typeof(NoticiaStatus), status));
            
            ViewBag.Orden = orden;
            ViewBag.OrdenNombre = string.IsNullOrEmpty(orden) ? "titulo_desc" : "";
            ViewBag.OrdenAutor = orden == "autor" ? "autor_desc" : "autor";
            ViewBag.OrdenVisitas = orden == "visitas" ? "visitas_desc" : "visitas";
            ViewBag.OrdenPublicacion = orden == "publicacion" ? "publicacion_desc" : "publicacion";
            ViewBag.OrdenMeGusta = orden == "megusta" ? "megusta_desc" : "megusta";
            if (buscar != null)
            {
                pagina = 1;
                buscar = buscar.Trim();
            }
            else { buscar = filtro ?? string.Empty; }
            ViewBag.Filtro = buscar;
            var noticias = db.Noticias
                .Include(n => n.Archivos)
                .Where(n => n.Status != NoticiaStatus.Ninguno);
            if (!string.IsNullOrEmpty(buscar) || myStatus != NoticiaStatus.Ninguno)
            {
                bool hayPalabras = !string.IsNullOrEmpty(buscar);

                // status = string.IsNullOrEmpty(status) ? NoticiaStatus.Ninguno.ToString() : status;
                noticias = noticias.Where(n =>
                    (
                        (!hayPalabras)
                        || n.Titulo.Contains(buscar)
                        || n.Resumen.Contains(buscar)
                        || n.HTMLContent.Contains(buscar)
                    )
                    && (myStatus == NoticiaStatus.Ninguno ? true : n.Status == myStatus)
                );
            }

            switch (orden)
            {
                case "titulo_desc":
                    noticias.OrderByDescending(n => n.Titulo);
                    break;
                case "autor":
                    noticias.OrderBy(n => n.Autor);
                    break;
                case "autor_desc":
                    noticias.OrderByDescending(n => n.Autor);
                    break;
                case "visitas":
                    noticias.OrderBy(n => n.ContadorVisitas);
                    break;
                case "visitas_desc":
                    noticias.OrderByDescending(n => n.ContadorVisitas);
                    break;
                case "publicacion":
                    noticias.OrderBy(n => n.FechaPublicacion);
                    break;
                case "publicacion_desc":
                    noticias.OrderByDescending(n => n.FechaPublicacion);
                    break;
                case "megusta":
                    noticias.OrderBy(n => n.MeGusta);
                    break;
                case "megusta_desc":
                    noticias.OrderByDescending(n => n.MeGusta);
                    break;
                default:
                    noticias.OrderBy(n => n.Titulo);
                    break;
            }

            List<NoticiaIndexListViewModel> noticiasViewModel = new List<NoticiaIndexListViewModel>();
            foreach (Noticia noticia in await noticias.ToListAsync())
            {
                noticiasViewModel.Add(new NoticiaIndexListViewModel(noticia));
            }
            ViewBag.Count = noticiasViewModel.Count();
            int numeroPagina = pagina ?? 1;
            int elementosPagina = Comun.ELEMENTOS_PAGINA;

            return View(noticiasViewModel.ToPagedList(numeroPagina, elementosPagina));
        } // Index

        // GET: Admin/Noticias/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                TempData["MessageBox"] = "No se recibió el identificador";
                if (Request.IsAjaxRequest()) return Content("BadRequest");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = await db.Noticias
                .Include(n => n.Archivos)
                .FirstOrDefaultAsync(n => n.NoticiaID == id);                
            if (noticia == null)
            {
                TempData["MessageBox"] = "No se encontró el registro del identificador";
                if (Request.IsAjaxRequest()) return Content("HttpNotFound");
                return HttpNotFound();
            }
            
            if (Request.IsAjaxRequest())
            {
                NoticiaDetailsViewModel n = new NoticiaDetailsViewModel(noticia, "details", true);
                return PartialView("_details", n);
            }
            return View(noticia);
         } // Details

        // GET: Admin/Noticias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Noticias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "NoticiaID,Titulo,Resumen,HTMLContent,Autor,TieneGaleria,ContadorVisitas,Idioma,FechaPublicacion,ImagenArchivo,MeGusta,Status,FechaCreacion,FechaActualizacion,UsuarioActualizacion")] Noticia noticia)
        {
            if (ModelState.IsValid)
            {
                noticia.NoticiaID = Guid.NewGuid();
                db.Noticias.Add(noticia);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(noticia);
        }

        // GET: Admin/Noticias/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = await db.Noticias.FindAsync(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        // POST: Admin/Noticias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "NoticiaID,Titulo,Resumen,HTMLContent,Autor,TieneGaleria,ContadorVisitas,Idioma,FechaPublicacion,ImagenArchivo,MeGusta,Status,FechaCreacion,FechaActualizacion,UsuarioActualizacion")] Noticia noticia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(noticia).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(noticia);
        }

        // GET: Admin/Noticias/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = await db.Noticias.FindAsync(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        // POST: Admin/Noticias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Noticia noticia = await db.Noticias.FindAsync(id);
            db.Noticias.Remove(noticia);
            await db.SaveChangesAsync();
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
    }
}
