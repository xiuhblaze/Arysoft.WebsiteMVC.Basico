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

namespace Arysoft.WebsiteMVC.Basico.Areas.Admin.Controllers
{
    public class NoticiasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Noticias
        public async Task<ActionResult> Index()
        {
            return View(await db.Noticias.ToListAsync());
        }

        // GET: Admin/Noticias/Details/5
        public async Task<ActionResult> Details(Guid? id)
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
