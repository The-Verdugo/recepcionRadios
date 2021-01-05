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
    public class UserController : Controller
    {
        private RecepcionDeRadiosContext db = new RecepcionDeRadiosContext();

        // GET: User
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Username,Password,Level,Active")] User user)
        {
            if (ModelState.IsValid)
            {
                user.ID = Guid.NewGuid().ToString();
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Active = 1;
                db.Users.Add(user);
                db.SaveChanges();
                var rol = new UserRolesMapping();
                rol.RoleId = user.Level;
                rol.UserId = user.ID;
                db.UserRolesMapping.Add(rol);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Username,Password,Level,Active")] User user)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                    return HttpNotFound();
                var currentRole = db.UserRolesMapping.Where(r => r.UserId == user.ID).FirstOrDefault();
                if (currentRole == null)
                    return HttpNotFound();
                currentRole.RoleId = user.Level;
                db.Entry(user).State = EntityState.Modified;
                if (user.Password != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }
                else
                {
                    db.Entry(user).Property(p => p.Password).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Desactivar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                user.Active = 0;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        public ActionResult Activar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                user.Active = 1;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
