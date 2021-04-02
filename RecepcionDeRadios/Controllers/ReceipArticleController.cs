using RecepcionDeRadios.DAL;
using RecepcionDeRadios.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RecepcionDeRadios.Controllers
{
    public class ReceipArticleController : Controller
    {
        private RecepcionDeRadiosContext db = new RecepcionDeRadiosContext();

        // GET: ReceipArticle
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.All = true;
            return View(db.ReceipArticles.ToList());
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Index(string dato,int criterio)
        {
            switch (criterio)
            {
                case 1: if (String.IsNullOrEmpty(dato))
                        {
                            ViewBag.All = true;
                            return View(await db.ReceipArticles.ToListAsync());
                        }
                        else
                        {
                            ViewBag.All = false;
                            ViewBag.Colab = dato;
                            return View(await db.ReceipArticles.Where(c => c.ID.ToString().Contains(dato)).ToListAsync());
                        }
                case 2:
                        if (String.IsNullOrEmpty(dato))
                        {
                            ViewBag.All = true;
                            return View(await db.ReceipArticles.ToListAsync());
                        }
                        else
                        {
                            ViewBag.All = false;
                            ViewBag.Colab = dato;

                            return View(await db.ReceipArticles.Where(c => c.empleadoEntrega.ToLower().Contains(dato)).ToListAsync());
                        }
                default: return View(await db.ReceipArticles.ToListAsync());
            }
            
            
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
            ViewBag.USER = db.Users.Single(b => b.ID == receipArticle.usuarioRecibe);
            return View(receipArticle);
        }

        // GET: ReceipArticle/Create
        public ActionResult Create()
        {
            ViewBag.URLEmpl = Environment.GetEnvironmentVariable("URLEMPLEADOS");
            ViewBag.URLArt = Environment.GetEnvironmentVariable("URLARTICULOS");
            return View();
        }

        // POST: ReceipArticle/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult Create([Bind(Include = "empleadoEntrega")] ReceipArticle receipArticle, FormCollection collection)
        // {

        //     if (ModelState.IsValid)
        //     {
        //         using (var dbContextTransaction = db.Database.BeginTransaction())
        //         {
        //             try
        //             {
        //                 ReceipArticle test = new ReceipArticle
        //                 {
        //                     fechaRecibido = DateTime.Now.Date,
        //                     usuarioRecibe = User.Identity.Name,
        //                     empleadoEntrega = receipArticle.empleadoEntrega,
        //                     fechaEntregado = DateTime.Now.Date

        //                 };

        //                 db.ReceipArticles.Add(test);
        //                 db.SaveChanges();
        //                 ReceipArticleDetail receipArticleDetail = new ReceipArticleDetail
        //                 {
        //                     ReceipArticleID = (from c in db.ReceipArticles orderby c.ID descending select c.ID).First(),
        //                     ReportedFailure = collection["Falla"],
        //                     ArticleID = collection["IdArticulo"]
        //                 };
        //                 db.ReceipArticleDetails.Add(receipArticleDetail);
        //                 db.SaveChanges();
        //                 dbContextTransaction.Commit();
        //             }catch (Exception e){ ModelState.AddModelError("RECEIP_ERROR", e.Message); return View(); }
        //         }
        //         return RedirectToAction("Index");
        //     }
                   
        //     ModelState.AddModelError("RECEIP_ERROR", "Please complete all fields");
        //     return View(receipArticle);
        // }
        [HttpPost]
        public JsonResult Create(ReceipArticle receip)
        {
            bool estado = false;
            
            try
                    {
                        ReceipArticle test = new ReceipArticle
                        {
                            fechaRecibido = DateTime.Now,
                            usuarioRecibe = User.Identity.Name,
                            fechaEntregado = DateTime.Now,
                            empleadoEntrega = receip.empleadoEntrega
                        };
                        db.ReceipArticles.Add(test);
                        db.SaveChanges();
                        var ReceipArticleID = (from c in db.ReceipArticles orderby c.ID descending select c.ID).First();
                        // ReceipArticleDetail receipArticleDetail = new ReceipArticleDetail
                        // {
                        //     ReceipArticleID = (from c in db.ReceipArticles orderby c.ID descending select c.ID).First()
                        //     ArticleID 
                        // };
                        foreach (ReceipArticleDetail article in receip.ReceipArticleDetails){
                            var des = article.Descripcion;
                            article.ReceipArticleID = ReceipArticleID;
                            article.Status= 1;
                            db.ReceipArticleDetails.Add(article);
                        }
                        db.SaveChanges();
                        estado= true;

                    }catch (Exception e){ ModelState.AddModelError("RECEIP_ERROR", e.Message); return new JsonResult { Data = new { estado } }; }
            return new JsonResult { Data = new { estado } };
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
