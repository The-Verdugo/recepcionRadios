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
    public class CommentsController : Controller
    {
        private RecepcionDeRadiosContext db = new RecepcionDeRadiosContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.ReceipArticleDetail);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.ReceipArticleDetailID = new SelectList(db.ReceipArticleDetails, "Id", "ArticleID");
            return View();
        }

        // POST: Comments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Create([Bind(Include = "ID,ReceipArticleDetailID,Username,Subject,Content,fecha")] Comment comment)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         db.Comments.Add(comment);
        //         db.SaveChanges();
        //         return RedirectToAction("Index");
        //     }

        //     ViewBag.ReceipArticleDetailID = new SelectList(db.ReceipArticleDetails, "Id", "ArticleID", comment.ReceipArticleDetailID);
        //     return View(comment);
        // }

        [HttpPost]
        public JsonResult Create(Comment comment)
        {
            bool estado = false;
            
            try
            {
                Comment comment1 = new Comment
                {
                    ReceipArticleDetailID = comment.ReceipArticleDetailID,
                    Username = comment.Username,
                    Subject = comment.Subject,
                    Content = comment.Content,
                    fecha = comment.fecha
                };
                db.Comments.Add(comment1);
                db.SaveChanges();
                estado= true;
            }
            catch (Exception e){ ModelState.AddModelError("RECEIP_ERROR", e.Message); return new JsonResult { Data = new { estado } }; }
            return new JsonResult { Data = new { estado } };
        }
        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReceipArticleDetailID = new SelectList(db.ReceipArticleDetails, "Id", "ArticleID", comment.ReceipArticleDetailID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ReceipArticleDetailID,Username,Subject,Content,fecha")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReceipArticleDetailID = new SelectList(db.ReceipArticleDetails, "Id", "ArticleID", comment.ReceipArticleDetailID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
