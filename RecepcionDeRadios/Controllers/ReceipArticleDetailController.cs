using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RecepcionDeRadios.DAL;
using RecepcionDeRadios.Models;

namespace RecepcionDeRadios.Controllers
{
    public class ReceipArticleDetailController : Controller
    {
        private RecepcionDeRadiosContext db = new RecepcionDeRadiosContext();
        string BaseUrl = "http://10.23.20.40:8080/api/articulos/";
        // GET: ReceipArticleDetail
        public ActionResult Index()
        {
            foreach (var item in db.ReceipArticleDetails.ToList())
            {
                var id = item.ArticleID;
            }
            return View(db.ReceipArticleDetails.ToList());
        }

        // GET: ReceipArticleDetail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceipArticleDetail receipArticleDetail = db.ReceipArticleDetails.Find(id);
            if (receipArticleDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdArticle = id;
            return View(receipArticleDetail);
        }

        // GET: ReceipArticleDetail/Create
        public ActionResult Create()
        {
            ViewBag.ReceipArticleID = new SelectList(db.ReceipArticles, "Id", "usuarioRecibe");
            return View();
        }

        // POST: ReceipArticleDetail/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReceipArticleID,ArticleID,Status")] ReceipArticleDetail receipArticleDetail)
        {
            if (ModelState.IsValid)
            {
                db.ReceipArticleDetails.Add(receipArticleDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReceipArticleID = new SelectList(db.ReceipArticles, "Id", "usuarioRecibe", receipArticleDetail.ReceipArticleID);
            return View(receipArticleDetail);
        }

        // GET: ReceipArticleDetail/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceipArticleDetail receipArticleDetail = db.ReceipArticleDetails.Find(id);
            if (receipArticleDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BOOL = false;
            ViewBag.ReceipArticleID = new SelectList(db.ReceipArticles, "Id", "usuarioRecibe", receipArticleDetail.ReceipArticleID);
            return View(receipArticleDetail);
        }

        // POST: ReceipArticleDetail/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReceipArticleID,ArticleID,Status,ReportedFailure,Descripcion")] ReceipArticleDetail receipArticleDetail)
        {
            ViewBag.BOOL = false;
            if (ModelState.IsValid)
            {
                db.Entry(receipArticleDetail).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.BOOL = true;
                return RedirectToAction("Details","ReceipArticleDetail", new { id=receipArticleDetail.ReceipArticleID });
            }
            ViewBag.ReceipArticleID = new SelectList(db.ReceipArticles, "Id", "usuarioRecibe", receipArticleDetail.ReceipArticleID);
            return View(receipArticleDetail);
        }

        // GET: ReceipArticleDetail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceipArticleDetail receipArticleDetail = db.ReceipArticleDetails.Find(id);
            if (receipArticleDetail == null)
            {
                return HttpNotFound();
            }
            return View(receipArticleDetail);
        }

        // POST: ReceipArticleDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReceipArticleDetail receipArticleDetail = db.ReceipArticleDetails.Find(id);
            db.ReceipArticleDetails.Remove(receipArticleDetail);
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
