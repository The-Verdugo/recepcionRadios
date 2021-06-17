using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecepcionDeRadios.DAL;
using RecepcionDeRadios.Models;
using EntityState = System.Data.Entity.EntityState;

namespace RecepcionDeRadios.Controllers
{
    [Authorize]
    public class BitacoraController : Controller
    {
        private RecepcionDeRadiosContext db = new RecepcionDeRadiosContext();

        // GET: Bitacora
        public ActionResult Index()
        {
            return View(db.ChangeLogs.ToList());
        }

        // GET: Bitacora/Details/5
        public ActionResult Details(int? id)
        {
            User usuario = new User();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = db.ChangeLogs.Find(id);
            usuario = db.Users.Find(bitacora.IdUsuario);
            if (bitacora == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserEdit = usuario;
            return View(bitacora);
        }

        // GET: Bitacora/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bitacora/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,accion,Tabla,PropertyModified,PrimaryKeyValue,OldValue,NewValue,IdUsuario,Fecha")] Bitacora bitacora)
        {
            if (ModelState.IsValid)
            {
                db.ChangeLogs.Add(bitacora);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bitacora);
        }

        // GET: Bitacora/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = db.ChangeLogs.Find(id);
            if (bitacora == null)
            {
                return HttpNotFound();
            }
            return View(bitacora);
        }

        // POST: Bitacora/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,accion,Tabla,PropertyModified,PrimaryKeyValue,OldValue,NewValue,IdUsuario,Fecha")] Bitacora bitacora)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bitacora).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bitacora);
        }

        // GET: Bitacora/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = db.ChangeLogs.Find(id);
            if (bitacora == null)
            {
                return HttpNotFound();
            }
            return View(bitacora);
        }

        // POST: Bitacora/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bitacora bitacora = db.ChangeLogs.Find(id);
            db.ChangeLogs.Remove(bitacora);
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
    }
}
