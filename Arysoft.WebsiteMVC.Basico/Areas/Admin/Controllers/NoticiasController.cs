using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
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

            ViewBag.MenuActive = "noticias";

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
                .Include(n => n.Notas)
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
        public async Task<ActionResult> Create()
        {
            await EliminarRegistrosTemporalesAsync(User.Identity.Name);

            Noticia noticia = new Noticia
            {
                NoticiaID = Guid.NewGuid(),
                Titulo = string.Empty,
                HTMLContent = string.Empty,
                TieneGaleria = BoolTipo.Ninguno,
                ContadorVisitas = 0,
                Idioma = IdiomaTipo.Ninguno,
                FechaPublicacion = DateTime.Now,
                MeGusta = 0,
                Status = NoticiaStatus.Ninguno,                
                FechaCreacion = DateTime.Now,                
                FechaActualizacion = DateTime.Now,
                UsuarioActualizacion = User.Identity.Name
            };

            db.Noticias.Add(noticia);

            try
            {
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = noticia.NoticiaID });
            }
            catch (Exception e)
            {
                TempData["MessageBox"] = "Ha ocurrido una excepción: " + e.Message;
                return RedirectToAction("Index");
            }
        } // Create

        //// POST: Admin/Noticias/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "NoticiaID,Titulo,Resumen,HTMLContent,Autor,TieneGaleria,ContadorVisitas,Idioma,FechaPublicacion,ImagenArchivo,MeGusta,Status,FechaCreacion,FechaActualizacion,UsuarioActualizacion")] Noticia noticia)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        noticia.NoticiaID = Guid.NewGuid();
        //        db.Noticias.Add(noticia);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(noticia);
        //}

        // GET: Admin/Noticias/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                TempData["MessageBox"] = "No se recibió el identificador.";
                return RedirectToAction("Index");
            }
            Noticia noticia = await db.Noticias
                .Include(n => n.Archivos)
                .Include(n => n.Notas)
                .FirstOrDefaultAsync(n => n.NoticiaID == id);
            if (noticia == null)
            {
                TempData["MessageBox"] = "No se encontró el registro.";
                return RedirectToAction("Index");
                //return HttpNotFound();
            }
            return View(new NoticiaEditViewModel(noticia));
        } // Edit

        // POST: Admin/Noticias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "NoticiaID,Titulo,__resumen,__content,Autor,TieneGaleria,ContadorVisitas,Idioma,FechaPublicacion,ImagenArchivo,MeGusta,Status,FechaCreacion")] NoticiaEditViewModel noticiaVM)
        {
            Noticia noticia = noticiaVM.ObtenerNoticia();

            bool esNuevo = noticia.Status == NoticiaStatus.Ninguno;

            if (esNuevo) {
                noticia.Status = NoticiaStatus.Nueva;
            }

            if (ModelState.IsValid)
            {
                noticia.FechaActualizacion = DateTime.Now;
                noticia.UsuarioActualizacion = User.Identity.Name;
                db.Entry(noticia).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (esNuevo) { TempData["MessageBox"] = "Registro guardado satisfactoriamente."; }
                else { TempData["MessageBox"] = "Cambios guardados con exito."; }

                return RedirectToAction("Index");
            }
            return View(noticia);
        } // Edit

        // GET: Admin/Noticias/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                TempData["MessageBox"] = "No se recibió el identificador";
                if (Request.IsAjaxRequest()) return Content("BadRequest");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = await db.Noticias
                .Include(n => n.Archivos)
                .Include(n => n.Notas)
                .FirstOrDefaultAsync(n => n.NoticiaID == id);
            if (noticia == null)
            {
                TempData["MessageBox"] = "No se encontró el registro del identificador";
                if (Request.IsAjaxRequest()) return Content("HttpNotFound");
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                NoticiaDetailsViewModel n = new NoticiaDetailsViewModel(noticia, "delete", true);
                return PartialView("_details", n);
            }
            return View(noticia);
        } // Delete

        // POST: Admin/Noticias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Noticia noticia = await db.Noticias.FindAsync(id);

            if (noticia.Status == NoticiaStatus.Eliminada)
            {
                // HACK: Falta remover los archivos y las notas asociadas a la noticia
                db.Noticias.Remove(noticia);
            }
            else
            {
                noticia.Status = NoticiaStatus.Eliminada;
                noticia.FechaActualizacion = DateTime.Now;
                noticia.UsuarioActualizacion = User.Identity.Name;
                db.Entry(noticia).State = EntityState.Modified;
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        } // DeleteConfirmed

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Activar(Guid id)
        {
            if (id == null)
            {
                TempData["MessageBox"] = "No se recibió el identificador";
                if (Request.IsAjaxRequest()) return Content("BadRequest");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = await db.Noticias.FindAsync(id);
            if (noticia == null)
            {
                TempData["MessageBox"] = "No se encontró el registro del identificador";
                if (Request.IsAjaxRequest()) return Content("HttpNotFound");
                return HttpNotFound();
            }
            noticia.Status = NoticiaStatus.Nueva;
            noticia.FechaActualizacion = DateTime.Now;
            noticia.UsuarioActualizacion = User.Identity.Name;
            db.Entry(noticia).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                TempData["MessageBox"] = "A ocurrido una excepción: " + e.Message;
                return RedirectToAction("Index");
            }
            TempData["MessageBox"] = "La noticia ha sido reactivada.";

            return RedirectToAction("Index");
        } // Activar

        [HttpPost]
        public async Task<ActionResult> AgregarImagenPrincipal(Guid id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    // Get all files from Request object
                    HttpFileCollectionBase files = Request.Files;
                    string fname = id.ToString();
                    string fextension = string.Empty;

                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        //// Checking for Internet Explorer
                        //if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        //{
                        //    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        //    fname = testfiles[testfiles.Length - 1];
                        //}
                        //else 
                        //{
                        //    fname = file.FileName;
                        //}
                        fextension = Path.GetExtension(file.FileName).ToLower();

                        // Get the complete folder path and store the file inside it.
                        fname = Path.Combine(Server.MapPath("~/Archivos/Noticias/"), fname + fextension);
                        file.SaveAs(fname);
                    }

                    // HACK: Actualizar el nombre del archivo en la base de datos

                    return Json("File uploaded successfully!");
                    //for (int i = 0; i < files.Count; i++)
                    //{
                    //    HttpPostedFileBase file = files[i];
                    //}
                }
                catch (Exception e)
                {
                    return Json("A ocurrido una excepción: " + e.Message);
                }
            }
            else
            {
                return Json("No se seleccionó ningun archivo.");
            }
        } // AgregarImagenPrincipal

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // METODOS PRIVADOS

        private async Task EliminarRegistrosTemporalesAsync(string userName)
        {
            var noticias = await (from n in db.Noticias
                where n.Status == NoticiaStatus.Ninguno
                && string.Compare(n.UsuarioActualizacion, userName, true) == 0
                select n).ToListAsync();

            foreach (var noticia in noticias)
            {
                db.Noticias.Remove(noticia);
            }
            await db.SaveChangesAsync();
        } // EliminarRegistrosTemporalesAsync
    }
}
