using RecepcionDeRadios.DAL;
using RecepcionDeRadios.Models;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
        public ActionResult Index() //Controlador de tipo GET para el index de articulos recibidos 
        {
            ViewBag.All = true; //Asigacion de valor booleano al Viewbag para enviar a la vista
            return View(db.ReceipArticles.ToList()); //Devuelve la vista con todos los registro en forma de lista 
        }

        //GET: Watch
        public ActionResult Watch() //controlador de tipo GET para index de pagina de consulta pública
        {
            ViewBag.All = true; //Asigacion de valor booleano al Viewbag para enviar a la vista
            return View(db.ReceipArticles.ToList()); //Devuelve la vista con todos los registro en forma de lista 
            
        }

        //Controlador asincrono de tipo POST para busqueda pública 
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Watch(int? colaborador, int? Folio, DateTime? fechaInicio, DateTime? fechaFin ) //Parametros opcionales de busqueda
        {
            
            if (!String.IsNullOrEmpty(Folio.ToString()) ) //Verificacion de que el folio contenga un dato 
            {
                ViewBag.Folio = Folio.ToString(); //Asignacion a una variable en el Viewbag de folio recibido para regresarlo a la vista 
                return View(await db.ReceipArticles.Where(c => c.ID.ToString().Contains(Folio.ToString())).ToListAsync()); //Devuelve el objeto con todos los datos de la consuta que se realizó
            } else if (!String.IsNullOrEmpty(colaborador.ToString())) //Verificación de que el numero de colaborador contenga un dato
            {
                if (!String.IsNullOrEmpty(fechaInicio.ToString()))
                {
                    if (!String.IsNullOrEmpty(fechaFin.ToString()))
                    {
                        DateTime inicio = Convert.ToDateTime(fechaInicio), fin = Convert.ToDateTime(fechaFin);
                        var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) >= inicio.Date && DbFunctions.TruncateTime(p.fechaRecibido) <= fin.Date && p.empleadoEntrega.Contains(colaborador.ToString()) select p;
                        ViewBag.Fin = fin.ToString("yyyy-MM-dd");
                        ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                        ViewBag.Busqueda = colaborador.ToString();
                        return View(await model.ToListAsync());
                    }
                    else { 
                        DateTime inicio = Convert.ToDateTime(fechaInicio);
                        var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) >= inicio.Date && p.empleadoEntrega.Contains(colaborador.ToString()) select p;
                        ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                        ViewBag.Busqueda = colaborador.ToString();
                        return View(await model.ToListAsync());
                    }
                }
                else 
                {
                    if (!String.IsNullOrEmpty(fechaFin.ToString()))
                    {
                        DateTime fin = Convert.ToDateTime(fechaFin);
                        var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) <= fin.Date && p.empleadoEntrega.Contains(colaborador.ToString()) select p;
                        ViewBag.Fin = fin.ToString("yyyy-MM-dd");
                        ViewBag.Busqueda = colaborador.ToString();
                        return View(await model.ToListAsync());
                    }
                    else
                    {
                        ViewBag.Busqueda = colaborador.ToString();
                        return View(await db.ReceipArticles.Where(c => c.empleadoEntrega.ToString().Contains(colaborador.ToString())).ToListAsync());
                    }
                    
                }
               
            }
            else if (!String.IsNullOrEmpty(fechaInicio.ToString()))
            {
                if (!String.IsNullOrEmpty(fechaFin.ToString()))
                {
                    DateTime inicio = Convert.ToDateTime(fechaInicio), fin = Convert.ToDateTime(fechaFin);
                    var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) >= inicio.Date && DbFunctions.TruncateTime(p.fechaRecibido) <= fin.Date select p;
                    ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                    ViewBag.Fin = fin.ToString("yyyy-MM-dd");
                    return View(await model.ToListAsync());
                }
                else
                {
                    DateTime inicio = Convert.ToDateTime(fechaInicio);
                    var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) >= inicio.Date select p;
                    ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                    return View(await model.ToListAsync());
                }
                
            }
            else if (!String.IsNullOrEmpty(fechaFin.ToString()))
            {
                    DateTime Fin = Convert.ToDateTime(fechaFin);
                    var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) <= Fin.Date select p;
                    ViewBag.Fin = Fin.ToString("yyyy-MM-dd");
                    return View(await model.ToListAsync());

            }

            return View();
        }

        //Controlador asincorno de tipo POST para el index de los articulos recibidos (Y Busqueda)
        [Authorize]
        [ValidateAntiForgeryToken] //Token de validacion del formulario 
        [HttpPost]
        public async Task<ActionResult> Index(int? colaborador, int? Folio, DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!String.IsNullOrEmpty(Folio.ToString())) //Verificacion de que el folio contenga un dato 
            {
                if (!String.IsNullOrEmpty(colaborador.ToString()))
                {
                    if (!String.IsNullOrEmpty(fechaInicio.ToString()))
                    {
                        if (!String.IsNullOrEmpty(fechaFin.ToString()))
                        {
                            DateTime inicio = Convert.ToDateTime(fechaInicio), fin = Convert.ToDateTime(fechaFin);
                            ViewBag.Fin = fin.ToString("yyyy-MM-dd");
                            ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                            ViewBag.Busqueda = colaborador.ToString();
                            ViewBag.Folio = Folio.ToString();
                            return View(await db.ReceipArticles.Where(c => DbFunctions.TruncateTime(c.fechaRecibido) >= inicio.Date && DbFunctions.TruncateTime(c.fechaRecibido) <= fin.Date && c.ID.ToString().Contains(Folio.ToString()) && c.empleadoEntrega.ToString().Contains(colaborador.ToString())).ToListAsync());
                        }
                        else
                        {
                            DateTime inicio = Convert.ToDateTime(fechaInicio);
                            ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                            ViewBag.Busqueda = colaborador.ToString();
                            ViewBag.Folio = Folio.ToString();
                            return View(await db.ReceipArticles.Where(c => DbFunctions.TruncateTime(c.fechaRecibido) >= inicio.Date && c.ID.ToString().Contains(Folio.ToString()) && c.empleadoEntrega.ToString().Contains(colaborador.ToString())).ToListAsync());
                        }

                    }
                    else 
                    {
                        ViewBag.Folio = Folio.ToString();
                        ViewBag.Busqueda = colaborador.ToString();
                        return View(await db.ReceipArticles.Where(c => c.ID.ToString().Contains(Folio.ToString()) && c.empleadoEntrega.ToString().Contains(colaborador.ToString())).ToListAsync());
                    }
                    
                }
                else if (!String.IsNullOrEmpty(fechaInicio.ToString()))
                {
                    if (!String.IsNullOrEmpty(fechaFin.ToString()))
                    {
                        DateTime inicio = Convert.ToDateTime(fechaInicio), fin = Convert.ToDateTime(fechaFin);
                        ViewBag.Fin = fin.ToString("yyyy-MM-dd");
                        ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                        ViewBag.Folio = Folio.ToString();
                        return View(await db.ReceipArticles.Where(c => DbFunctions.TruncateTime(c.fechaRecibido) >= inicio.Date && DbFunctions.TruncateTime(c.fechaRecibido) <= fin.Date && c.ID.ToString().Contains(Folio.ToString())).ToListAsync());
                    }
                    else
                    {
                        DateTime inicio = Convert.ToDateTime(fechaInicio);
                        ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                        ViewBag.Folio = Folio.ToString();
                        return View(await db.ReceipArticles.Where(c => DbFunctions.TruncateTime(c.fechaRecibido) >= inicio.Date && c.ID.ToString().Contains(Folio.ToString())).ToListAsync());
                    }
                }
                else if (!String.IsNullOrEmpty(fechaFin.ToString()))
                {
                    DateTime fin = Convert.ToDateTime(fechaFin);
                    ViewBag.Fin = fin.ToString("yyyy-MM-dd");
                    ViewBag.Folio = Folio.ToString();
                    return View(await db.ReceipArticles.Where(c => DbFunctions.TruncateTime(c.fechaRecibido) <= fin.Date && c.ID.ToString().Contains(Folio.ToString())).ToListAsync());
                }
                else 
                { 
                ViewBag.Folio = Folio.ToString(); //Asignacion a una variable en el Viewbag de folio recibido para regresarlo a la vista 
                return View(await db.ReceipArticles.Where(c => c.ID.ToString().Contains(Folio.ToString())).ToListAsync()); //Devuelve el objeto con todos los datos de la consuta que se realizó
                }
            }
            else if (!String.IsNullOrEmpty(colaborador.ToString())) //Verificación de que el numero de colaborador contenga un dato
            {
                if (!String.IsNullOrEmpty(fechaInicio.ToString()))
                {
                    if (!String.IsNullOrEmpty(fechaFin.ToString()))
                    {
                        DateTime inicio = Convert.ToDateTime(fechaInicio), fin = Convert.ToDateTime(fechaFin);
                        var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) >= inicio.Date && DbFunctions.TruncateTime(p.fechaRecibido) <= fin.Date && p.empleadoEntrega.Contains(colaborador.ToString()) select p;
                        ViewBag.Fin = fin.ToString("yyyy-MM-dd");
                        ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                        ViewBag.Busqueda = colaborador.ToString();
                        return View(await model.ToListAsync());
                    }
                    else
                    {
                        DateTime inicio = Convert.ToDateTime(fechaInicio);
                        var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) >= inicio.Date && p.empleadoEntrega.Contains(colaborador.ToString()) select p;
                        ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                        ViewBag.Busqueda = colaborador.ToString();
                        return View(await model.ToListAsync());
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(fechaFin.ToString()))
                    {
                        DateTime fin = Convert.ToDateTime(fechaFin);
                        var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) <= fin.Date && p.empleadoEntrega.Contains(colaborador.ToString()) select p;
                        ViewBag.Fin = fin.ToString("yyyy-MM-dd");
                        ViewBag.Busqueda = colaborador.ToString();
                        return View(await model.ToListAsync());
                    }
                    else
                    {
                        ViewBag.Busqueda = colaborador.ToString();
                        return View(await db.ReceipArticles.Where(c => c.empleadoEntrega.ToString().Contains(colaborador.ToString())).ToListAsync());
                    }

                }

            }
            else if (!String.IsNullOrEmpty(fechaInicio.ToString()))
            {
                if (!String.IsNullOrEmpty(fechaFin.ToString()))
                {
                    DateTime inicio = Convert.ToDateTime(fechaInicio), fin = Convert.ToDateTime(fechaFin);
                    var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) >= inicio.Date && DbFunctions.TruncateTime(p.fechaRecibido) <= fin.Date select p;
                    ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                    ViewBag.Fin = fin.ToString("yyyy-MM-dd");
                    return View(await model.ToListAsync());
                }
                else
                {
                    DateTime inicio = Convert.ToDateTime(fechaInicio);
                    var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) >= inicio.Date select p;
                    ViewBag.Inicio = inicio.ToString("yyyy-MM-dd");
                    return View(await model.ToListAsync());
                }

            }
            else if (!String.IsNullOrEmpty(fechaFin.ToString()))
            {
                DateTime Fin = Convert.ToDateTime(fechaFin);
                var model = from p in db.ReceipArticles where DbFunctions.TruncateTime(p.fechaRecibido) <= Fin.Date select p;
                ViewBag.Fin = Fin.ToString("yyyy-MM-dd");
                return View(await model.ToListAsync());

            }

            return View(db.ReceipArticles.ToList());
        }

        //Controlador de tipo GET para la vista de detalles con usuario logueado 
        [Authorize]
        // GET: ReceipArticle/Details/5
        public ActionResult Details(int? id, bool obj= false)
        {
            if (obj)
            {
                ViewBag.BOOL = obj;
            }
            else {
                ViewBag.BOOL = obj;
            }
            if (id == null) // Verificacion de que el parametro contenga un dato 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //En caso que el parametro sea nulo se regresa un estado de error 
            }
            ReceipArticle receipArticle = db.ReceipArticles.Find(id); //Busqueda en la base de datos en base al parametro recibido 
            if (receipArticle == null) //Verifica que el objeto de la consulta no esté vacio 
            {
                return HttpNotFound(); //En caso de estar vacio regresa un error 404
            }
            ViewBag.USER = db.Users.Single(b => b.ID == receipArticle.usuarioRecibe); //Consulta y asignacion del nombre del usuario para enviar a la vista
            return View(receipArticle); //Regresa el objeto con los datos consultados a la vista
        }

        //Controlador de tipo GET para la vista de detalles pública 
        public ActionResult PublicDetails(int? id)
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

        public ActionResult Print(int? id) {
            ViewBag.Print = true;
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
            return new Rotativa.ActionAsPdf("PrintView", receipArticle) {
                PageMargins = new Margins(0, 0, 0, 0),
                PageSize = Size.B7,
                PageWidth = 70,
                PageHeight = 297,
                FileName = "Recepción #"+ receipArticle.ID +".pdf"

            };
        }
        // GET: ReceipArticle/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.URLEmpl = Environment.GetEnvironmentVariable("URLEMPLEADOS");
            ViewBag.URLArt = Environment.GetEnvironmentVariable("URLARTICULOS");
            return View();
        }

        [HttpPost]
        public JsonResult Create(ReceipArticle receip)
        {
            bool estado = false;
            var ReceipArticleID = 0;
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
                        ReceipArticleID = (from c in db.ReceipArticles orderby c.ID descending select c.ID).First();
                        foreach (ReceipArticleDetail article in receip.ReceipArticleDetails){
                            article.ReceipArticleID = ReceipArticleID;
                            article.Status= 1;
                            db.ReceipArticleDetails.Add(article);
                        }
                        db.SaveChanges();
                        estado= true;

            }
            catch (Exception e) { ModelState.AddModelError("RECEIP_ERROR", e.Message); return new JsonResult() { Data = new { estado } }; }
            return new JsonResult { Data = new { estado, ID = ReceipArticleID } };
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
        [Authorize(Roles ="Administrador")]
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

        public ActionResult PrintView(int? id) {
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

        // POST: ReceipArticle/Delete/5
        [Authorize(Roles ="Administrador")]
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
