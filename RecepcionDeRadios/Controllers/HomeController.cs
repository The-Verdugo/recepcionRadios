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
                Session["User"] = usuario.User;
                Session.Timeout = 60*24;
                if (UserExists != null)
                {
                    if (UserExists.Active.Equals(2))
                    {
                        ModelState.AddModelError("LOGIN_ERROR", "El usuario con el que intenta acceder se encuentra desactivado");
                        return View();
                    }
                    else
                    {
                        bool IsValidUser = BCrypt.Net.BCrypt.Verify(usuario.Password, UserExists?.Password);

                        if (IsValidUser)
                        {
                            FormsAuthentication.SetAuthCookie(UserExists.ID, false);
                            return RedirectToAction("create", "receiparticle");
                        }
                    }
                }
            }
            ModelState.AddModelError("LOGIN_ERROR", "Usuario o contraseña incorrecto");
            return View();
        }

        public void Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            //return View();
        }
    }
}