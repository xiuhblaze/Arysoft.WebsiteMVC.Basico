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
                PaginaDetailsViewModel p = new PaginaDetailsViewModel(pagina, "details", true);
                return PartialView("_details", p);
            }
            return View(pagina);
        } // Details

        // GET: Admin/Paginas/Create
        public ActionResult Create()
        {
            ViewBag.PaginaPadreID = new SelectList(db.Paginas, "PaginaID", "Titulo");
            return View();
        }

        // POST: Admin/Paginas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PaginaID,PaginaPadreID,Titulo,EtiquetaMenu,Resumen,HtmlContent,Visible,TargetUrl,Target,TieneGaleria,ContadorVisitas,FechaContador,Idioma,EsPrincipal,HTMLHeadScript,HTMLFooterScript,MeGusta,Status,FechaCreacion,FechaActualizacion,UsuarioActualizacion")] Pagina pagina)
        {
            if (ModelState.IsValid)
            {
                pagina.PaginaID = Guid.NewGuid();
                db.Paginas.Add(pagina);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PaginaPadreID = new SelectList(db.Paginas, "PaginaID", "Titulo", pagina.PaginaPadreID);
            return View(pagina);
        }

        // GET: Admin/Paginas/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = await db.Paginas.FindAsync(id);
            if (pagina == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaginaPadreID = new SelectList(db.Paginas, "PaginaID", "Titulo", pagina.PaginaPadreID);
            return View(pagina);
        }

        // POST: Admin/Paginas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PaginaID,PaginaPadreID,Titulo,EtiquetaMenu,Resumen,HtmlContent,Visible,TargetUrl,Target,TieneGaleria,ContadorVisitas,FechaContador,Idioma,EsPrincipal,HTMLHeadScript,HTMLFooterScript,MeGusta,Status,FechaCreacion,FechaActualizacion,UsuarioActualizacion")] Pagina pagina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pagina).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PaginaPadreID = new SelectList(db.Paginas, "PaginaID", "Titulo", pagina.PaginaPadreID);
            return View(pagina);
        }

        // GET: Admin/Paginas/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagina pagina = await db.Paginas.FindAsync(id);
            if (pagina == null)
            {
                return HttpNotFound();
            }
            return View(pagina);
        }

        // POST: Admin/Paginas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Pagina pagina = await db.Paginas.FindAsync(id);
            db.Paginas.Remove(pagina);
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
