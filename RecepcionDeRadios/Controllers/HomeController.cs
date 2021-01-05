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
            return View();
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
                if (UserExists != null)
                {
                    bool IsValidUser = BCrypt.Net.BCrypt.Verify(usuario.Password, UserExists?.Password);

                    if (IsValidUser)
                    {
                        FormsAuthentication.SetAuthCookie(UserExists.ID, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "invalid Username or Password");
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