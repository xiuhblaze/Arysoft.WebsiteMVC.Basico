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
    public class ArchivosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Archivos
        public async Task<ActionResult> Index()
        {
            return View(await db.Archivos.ToListAsync());
        }

        // GET: Admin/Archivos/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = await db.Archivos.FindAsync(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            return View(archivo);
        }

        // GET: Admin/Archivos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Archivos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ArchivoID,PropietarioID,Nombre,Descripcion,Tags,Status,FechaAlta,FechaActualizacion,UsuarioActualizacion")] Archivo archivo)
        {
            if (ModelState.IsValid)
            {
                archivo.ArchivoID = Guid.NewGuid();
                db.Archivos.Add(archivo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(archivo);
        }

        // GET: Admin/Archivos/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = await db.Archivos.FindAsync(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            return View(archivo);
        }

        // POST: Admin/Archivos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ArchivoID,PropietarioID,Nombre,Descripcion,Tags,Status,FechaAlta,FechaActualizacion,UsuarioActualizacion")] Archivo archivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archivo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(archivo);
        }

        // GET: Admin/Archivos/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = await db.Archivos.FindAsync(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            return View(archivo);
        }

        // POST: Admin/Archivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Archivo archivo = await db.Archivos.FindAsync(id);
            db.Archivos.Remove(archivo);
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
