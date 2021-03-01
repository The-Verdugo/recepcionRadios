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

namespace RecepcionDeRadios.Controllers
{
    public class ReceipArticleController : Controller
    {
        private RecepcionDeRadiosContext db = new RecepcionDeRadiosContext();

        // GET: ReceipArticle
        public ActionResult Index()
        {
            return View(db.ReceipArticles.ToList());
        }

        // GET: ReceipArticle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceipArticle receipArticle = db.ReceipArticles.Find(id);
            if (receipArticle == null)
            {
                return HttpNotFound();
            }
            return View(receipArticle);
        }

        // GET: ReceipArticle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReceipArticle/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,usuarioRecibe,empleadoEntrega,fechaRecibido,fechaEntregado")] ReceipArticle receipArticle)
        {
            if (ModelState.IsValid)
            {
                db.ReceipArticles.Add(receipArticle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(receipArticle);
        }

        // GET: ReceipArticle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceipArticle receipArticle = db.ReceipArticles.Find(id);
            if (receipArticle == null)
            {
                return HttpNotFound();
            }
            return View(receipArticle);
        }

        // POST: ReceipArticle/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,usuarioRecibe,empleadoEntrega,fechaRecibido,fechaEntregado")] ReceipArticle receipArticle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receipArticle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(receipArticle);
        }

        // GET: ReceipArticle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceipArticle receipArticle = db.ReceipArticles.Find(id);
            if (receipArticle == null)
            {
                return HttpNotFound();
            }
            return View(receipArticle);
        }

        // POST: ReceipArticle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReceipArticle receipArticle = db.ReceipArticles.Find(id);
            db.ReceipArticles.Remove(receipArticle);
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
