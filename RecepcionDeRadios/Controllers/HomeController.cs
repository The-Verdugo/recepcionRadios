using RecepcionDeRadios.DAL;
using RecepcionDeRadios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RecepcionDeRadios.Controllers
{
    
    public class HomeController : Controller
    {
        private RecepcionDeRadiosContext db = new RecepcionDeRadiosContext();
      
        public ActionResult Index()
        {
            return RedirectToAction("create","receiparticle");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login usuario)
        {
            if (ModelState.IsValid)
            {
                var UserExists = db.Users.Where(u => u.Username == usuario.User).FirstOrDefault();
                var Name = (from c in db.Users select c.Name).First();
                if (UserExists != null)
                {
                    bool IsValidUser = BCrypt.Net.BCrypt.Verify(usuario.Password, UserExists?.Password);

                    if (IsValidUser)
                    {
                        ViewBag.Name = Name;
                        FormsAuthentication.SetAuthCookie(UserExists.ID, false);
                        return RedirectToAction("create","receiparticle");
                    }
                }
            }
            ModelState.AddModelError("LOGIN_ERROR", "Invalid Username or Password");
            return View();
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            //return View();
        }
    }
}