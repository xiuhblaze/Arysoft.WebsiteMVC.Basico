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
    public class PaginasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Paginas
        public async Task<ActionResult> Index(string buscar, string filtro, string orden, string status, int? pagina)
        {
            PaginaStatus myStatus = (PaginaStatus)(string.IsNullOrEmpty(status) ? PaginaStatus.Ninguno : Enum.Parse(typeof(PaginaStatus), status));

            ViewBag.MenuActive = "paginas";

            ViewBag.Orden = orden;
            ViewBag.OrdenTitulo = string.IsNullOrEmpty(orden) ? "titulo_desc" : "";
            ViewBag.OrdenFecha = orden == "fecha" ? "fecha_desc" : "fecha";
            ViewBag.OrdenVisitas = orden == "visitas" ? "visitas_desc" : "visitas";
            ViewBag.OrdenMeGusta = orden == "megusta" ? "megusta_desc" : "megusta";
            if (buscar != null)
            {
                pagina = 1;
                buscar = buscar.Trim();
            }
            else { buscar = filtro ?? string.Empty; }
            ViewBag.Filtro = buscar;
            var paginas = db.Paginas
                .Include(p => p.PaginaPadre)
                .Include(p => p.Archivos)
                .Include(p => p.Notas)
                .Where(p => p.Status != PaginaStatus.Ninguno);
            if (!string.IsNullOrEmpty(buscar) || myStatus != PaginaStatus.Ninguno)
            {
                bool hayPalabras = !string.IsNullOrEmpty(buscar);

                paginas = paginas.Where(p => 
                    (
                        (!hayPalabras)
                        || p.Titulo.Contains(buscar)
                        || p.Resumen.Contains(buscar)
                        || p.HTMLContent.Contains(buscar)
                    )
                    && (myStatus == PaginaStatus.Ninguno ? true : p.Status == myStatus)
                );
            }
            switch (orden)
            {
                case "titulo_desc":
                    paginas = paginas.OrderByDescending(p => p.Titulo);
                    break;
                case "fecha":
                    paginas = paginas.OrderBy(p => p.FechaActualizacion);
                    break;
                case "fecha_desc":
                    paginas = paginas.OrderByDescending(p => p.FechaActualizacion);
                    break;
                case "visitas":
                    paginas = paginas.OrderBy(p => p.ContadorVisitas);
                    break;
                case "visitas_desc":
                    paginas = paginas.OrderBy(p => p.ContadorVisitas);
                    break;
                case "megusta":
                    paginas = paginas.OrderBy(p => p.MeGusta);
                    break;
                case "megusta_desc":
                    paginas = paginas.OrderByDescending(p => p.MeGusta);
                    break;
                default:
                    paginas = paginas.OrderBy(p => p.Titulo);
                    break;
            }
            List<PaginaIndexListViewModel> paginasViewModel = new List<PaginaIndexListViewModel>();
            foreach (Pagina miPagina in await paginas.ToListAsync())
            {
                paginasViewModel.Add(new PaginaIndexListViewModel(miPagina));
            }
            ViewBag.Count = paginasViewModel.Count();
            int numeroPagina = pagina ?? 1;
            int elementosPagina = Comun.ELEMENTOS_PAGINA;

            return View(paginasViewModel.ToPagedList(numeroPagina, elementosPagina));
        } // Index

        // GET: Admin/Paginas/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                TempData["MessageBox"] = "No se recibió el identificador";
                if (Request.IsAjaxRequest()) return Content("BadRequest");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = await db.Paginas
                .Include(p => p.Archivos)
                .Include(p => p.Notas)
                .FirstOrDefaultAsync(p => p.PaginaID == id);
            if (pagina == null)
            {
                TempData["MessageBox"] = "No se encontró el registro del identificador";
                if (Request.IsAjaxRequest()) return Content("HttpNotFound");
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                TempData["isReadonly"] = true;
                TempData["isPage"] = true;
                PaginaDetailsViewModel p = new PaginaDetailsViewModel(pagina, "details", true);
                return PartialView("_details", p);
            }
            return View(pagina);
        } // Details

        // GET: Admin/Paginas/Create
        public async Task<ActionResult> Create()
        {
            await EliminarRegistrosTemporalesAsync(User.Identity.Name);

            Pagina pagina = new Pagina
            {
                PaginaID = Guid.NewGuid(),
                IndiceMenu = 0,
                Target = PaginaTarget.Ninguno,
                ContadorVisitas = 0,
                TieneGaleria = BoolTipo.Ninguno,
                FechaContador = DateTime.Now,
                Idioma = IdiomaTipo.Ninguno,
                EsPrincipal = BoolTipo.Ninguno,
                MeGusta = 0,
                Status = PaginaStatus.Ninguno,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now,
                UsuarioActualizacion = User.Identity.Name
            };

            db.Paginas.Add(pagina);

            try
            {
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = pagina.PaginaID });
            }
            catch (Exception e)
            {
                TempData["MessageBox"] = "Ha ocurrido una excepción: " + e.Message;
                return RedirectToAction("Index");
            }
        } // Create

        //// POST: Admin/Paginas/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "PaginaID,PaginaPadreID,Titulo,EtiquetaMenu,Resumen,HtmlContent,Visible,TargetUrl,Target,TieneGaleria,ContadorVisitas,FechaContador,Idioma,EsPrincipal,HTMLHeadScript,HTMLFooterScript,MeGusta,Status,FechaCreacion,FechaActualizacion,UsuarioActualizacion")] Pagina pagina)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        pagina.PaginaID = Guid.NewGuid();
        //        db.Paginas.Add(pagina);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.PaginaPadreID = new SelectList(db.Paginas, "PaginaID", "Titulo", pagina.PaginaPadreID);
        //    return View(pagina);
        //}

        // GET: Admin/Paginas/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                TempData["MessageBox"] = "No se recibió el identificador.";
                return RedirectToAction("Index");
            }
            Pagina pagina = await db.Paginas
                .Include(p => p.Archivos)
                .Include(p => p.Notas)
                .FirstOrDefaultAsync(p => p.PaginaID == id);
            if (pagina == null)
            {
                TempData["MessageBox"] = "No se encontró el registro.";
                return RedirectToAction("Index");
                //return HttpNotFound();
            }
            if (pagina.Status == PaginaStatus.Eliminada)
            {
                TempData["MessageBox"] = "La página ha sido eliminada.";
                return RedirectToAction("Index");
            }
            TempData["isReadonly"] = false;
            TempData["isPage"] = true;
            var paginasPadreList = new SelectList(db.Paginas
                .Where(p => p.Status == PaginaStatus.Publicada), 
                "PaginaID", 
                "Titulo", 
                pagina.PaginaPadreID)
            .ToList();
            paginasPadreList.Insert(0, new SelectListItem { Text = "(es página padre)", Value = Guid.Empty.ToString() });
            ViewBag.PaginaPadreID = paginasPadreList;

            return View(new PaginaEditViewModel(pagina));
        } // Edit

        // POST: Admin/Paginas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PaginaID,PaginaPadreID,Titulo,EtiquetaMenu,__resumen,__content,TargetUrl,Target,TieneGaleria,ContadorVisitas,FechaContador,Idioma,EsPrincipal,__headScript,__footerScript,MeGusta,Status,FechaCreacion")] PaginaEditViewModel paginaVM)
        {
            Pagina pagina = paginaVM.ObtenerPagina();

            bool esNuevo = pagina.Status == PaginaStatus.Ninguno;

            if (esNuevo)
            {
                pagina.Status = PaginaStatus.Nueva;
            }

            if (ModelState.IsValid)
            {
                pagina.FechaActualizacion = DateTime.Now;
                pagina.UsuarioActualizacion = User.Identity.Name;
                db.Entry(pagina).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (esNuevo) { TempData["MessageBox"] = "Registro guardado satisfactoriamente."; }
                else { TempData["MessageBox"] = "Cambios guardados con exito."; }

                return RedirectToAction("Index");
            }

            TempData["isReadonly"] = false;
            TempData["isPage"] = true;
            ViewBag.PaginaPadreID = new SelectList(db.Paginas, "PaginaID", "Titulo", pagina.PaginaPadreID);
            return View(pagina);
        } // Edit

        // GET: Admin/Paginas/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                TempData["MessageBox"] = "No se recibió el identificador";
                if (Request.IsAjaxRequest()) return Content("BadRequest");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = await db.Paginas
                .Include(p => p.Archivos)
                .Include(p => p.Notas)
                .FirstOrDefaultAsync(p => p.PaginaID == id);
            if (pagina == null)
            {
                TempData["MessageBox"] = "No se encontró el registro del identificador";
                if (Request.IsAjaxRequest()) return Content("HttpNotFound");
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                TempData["isReadonly"] = true;
                TempData["isPage"] = true;
                PaginaDetailsViewModel p = new PaginaDetailsViewModel(pagina, "delete", true);
                return PartialView("_details", p);
            }
            return View(pagina);
        } // Delete

        // POST: Admin/Paginas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Pagina pagina = await db.Paginas.FindAsync(id);

            if (pagina.Status == PaginaStatus.Eliminada)
            {
                // Eliminando la carpeta de archivos de la página
                string dname = Path.Combine(Server.MapPath("~/Archivos/Paginas/"), pagina.PaginaID.ToString());
                if (Directory.Exists(dname)) { Directory.Delete(dname, true); }
                // Eliminando registros asociados a la página
                foreach (Nota n in await db.Notas.Where(n => n.PropietarioID == pagina.PaginaID).ToListAsync())
                {
                    db.Notas.Remove(n);
                }
                foreach (Archivo a in await db.Archivos.Where(a => a.PropietarioID == pagina.PaginaID).ToListAsync())
                {
                    db.Archivos.Remove(a);
                }
                // Eliminando la página
                db.Paginas.Remove(pagina);
            }
            else
            {
                pagina.Status = PaginaStatus.Eliminada;
                pagina.FechaActualizacion = DateTime.Now;
                pagina.UsuarioActualizacion = User.Identity.Name;
                db.Entry(pagina).State = EntityState.Modified;
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
            Pagina pagina = await db.Paginas.FindAsync(id);
            if (pagina == null)
            {
                TempData["MessageBox"] = "No se encontró el registro del identificador";
                if (Request.IsAjaxRequest()) return Content("HttpNotFound");
                return HttpNotFound();
            }
            pagina.Status = PaginaStatus.Nueva;
            pagina.FechaActualizacion = DateTime.Now;
            pagina.UsuarioActualizacion = User.Identity.Name;
            db.Entry(pagina).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                TempData["MessageBox"] = "A ocurrido una excepción: " + e.Message;
                return RedirectToAction("Index");
            }
            TempData["MessageBox"] = "La página ha sido reactivada.";

            return RedirectToAction("Index");
        } // Activar

        // ARCHIVOS

        

        // NOTAS

        [HttpPost]
        public ActionResult AgregarNota(Guid id, string nota)
        {
            if (string.IsNullOrEmpty(nota))
            {
                return Json(new
                {
                    status = "notnote",
                    message = "No se encontró la nota."
                });
            }

            try
            {
                Nota miNota = new Nota()
                {
                    NotaID = Guid.NewGuid(),
                    PropietarioID = id,
                    Texto = nota,
                    Autor = "<nombre del autor>",
                    Status = StatusTipo.Activo,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    UsuarioActualizacion = User.Identity.Name
                };
                db.Notas.Add(miNota);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = "exception",
                    message = "A ocurrido una excepción: " + e.Message
                });
            }

            if (Request.IsAjaxRequest())
            {
                TempData["isReadonly"] = false;
                TempData["isPage"] = false;
                var notas = from n in db.Notas
                            where n.PropietarioID == id
                            orderby n.FechaCreacion descending
                            select n;
                return PartialView("~/Areas/Admin/Views/Notas/_notasList.cshtml", notas);
            }

            return Json(new
            {
                status = "unknow",
                message = "Esto no deberia llegar hasta aqui (por ahora)."
            });
        } // AgregarNota

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
            var paginas = await (from n in db.Paginas
                                  where n.Status == PaginaStatus.Ninguno
                                  && string.Compare(n.UsuarioActualizacion, userName, true) == 0
                                  select n).ToListAsync();

            foreach (var pagina in paginas)
            {
                string dname = Path.Combine(Server.MapPath("~/Archivos/Paginas/"), pagina.PaginaID.ToString());
                if (Directory.Exists(dname)) { Directory.Delete(dname, true); }

                foreach (Nota n in await db.Notas.Where(n => n.PropietarioID == pagina.PaginaID).ToListAsync())
                {
                    db.Notas.Remove(n);
                }
                foreach (Archivo a in await db.Archivos.Where(a => a.PropietarioID == pagina.PaginaID).ToListAsync())
                {
                    db.Archivos.Remove(a);
                }
                db.Paginas.Remove(pagina);
            }
            await db.SaveChangesAsync();
        } // EliminarRegistrosTemporalesAsync
    }
}
