using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using Newtonsoft.Json;
using RecepcionDeRadios.DAL;
using RecepcionDeRadios.Models;

namespace RecepcionDeRadios.Controllers
{
    public class ReceipArticleDetailController : Controller
    {
        private RecepcionDeRadiosContext db = new RecepcionDeRadiosContext();
        // GET: ReceipArticleDetail
        [Authorize]
        public ActionResult Index()
        {
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
            ViewBag.USER = db.Users.Single(b => b.ID == receipArticleDetail.ReceipArticle.usuarioRecibe);
            ViewBag.IdArticle = id;
            return View(receipArticleDetail);
        }

        public ActionResult PublicDetails(int? id)
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
            ViewBag.USER = db.Users.Single(b => b.ID == receipArticleDetail.ReceipArticle.usuarioRecibe);
            ViewBag.IdArticle = id;
            return View(receipArticleDetail);
        }

        // GET: ReceipArticleDetail/Create
        public ActionResult Create()
        {
            ViewBag.ReceipArticleID = new SelectList(db.ReceipArticles, "Id", "usuarioRecibe");
            return View();
        }

        [HttpPost]
        public JsonResult PaseSalida(string[] articlesid,string folio)
        {
            bool estado = false;
            try
            {
                foreach (var article in articlesid)
                {
                    ReceipArticleDetail receipArticleDetail = db.ReceipArticleDetails.Where(c => c.Id.ToString().Contains(article)).First();
                    receipArticleDetail.Status = 2;
                    receipArticleDetail.PaseSalida = folio;
                    receipArticleDetail.FechaEntregaProveedor = DateTime.Now;
                    db.Entry(receipArticleDetail).State = EntityState.Modified;
                }
                db.SaveChanges();
                estado = true;

            }
            catch (Exception e) { ModelState.AddModelError("RECEIP_ERROR", e.Message); return new JsonResult() { Data = new { estado } }; }
            return new JsonResult { Data = new { estado } };
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
        [Authorize(Roles = "Administrador ,Tecnico")]
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
        [Authorize(Roles = "Administrador ,Tecnico")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReceipArticleID,ArticleID,Status,Description,ReportedFailure,FechaEntregaUsuario,FechaEntregaProveedor,PaseSalida,FolioBaja")] ReceipArticleDetail receipArticleDetail)
        {
                
                db.Entry(receipArticleDetail).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.BOOL = true;

                return  RedirectToAction("Details","ReceipArticle", new { id=receipArticleDetail.ReceipArticleID});
            
        }
        
        public FileResult Export()
        {
            DataTable dt = new DataTable("Radios");
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("Id Artículo"),
                                            new DataColumn("Estatus"),
                                            new DataColumn("Falla"),
                                            new DataColumn("Descripción"),
                                            new DataColumn("Folio de salida"),
                                            new DataColumn("Folio de baja"),
                                            new DataColumn("Fecha de entrega al proveedor"),
                                            new DataColumn("Fecha de entrega al Usuario")
            });

            var articulos = db.ReceipArticleDetails.ToList();

            foreach (var articulo in articulos)
            {
                string fechaUs = articulo.FechaEntregaUsuario.ToString(),fechaProv= articulo.FechaEntregaProveedor.ToString(),baja = articulo.FolioBaja,salida= articulo.PaseSalida, status = articulo.Status.ToString();

                if (!articulo.FechaEntregaProveedor.HasValue)
                {
                    fechaProv = "N/D";
                }
                
                if (!articulo.FechaEntregaUsuario.HasValue)
                {
                    fechaUs = "N/D";
                }
                if (string.IsNullOrEmpty(baja))
                {
                    baja = "S/N";
                }
                if (string.IsNullOrEmpty(salida))
                {
                    salida = "S/N";
                }

                switch (status)
                {
                    case "1": status = "Entregado";
                        break;
                    case "2": status = "Entregado al proveedor";
                        break;
                    case "3": status = "Reparado";
                        break;
                    case "4": status = "Baja";
                        break;
                    case "5": status = "Entregado al Usuario";
                        break;
                    default:
                        break;
                }
                dt.Rows.Add(articulo.ArticleID,
                    status, 
                    articulo.ReportedFailure, 
                    articulo.Description, 
                    salida,
                    baja,
                    fechaProv,
                    fechaUs
                    );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                
                var worksheet = wb.Worksheets.Add(dt);
                var rango = worksheet.Range("A1:H1"); //Seleccionamos un rango
                rango.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick); //Generamos las lineas exteriores
                rango.Style.Border.SetInsideBorder(XLBorderStyleValues.Medium); //Generamos las lineas interiores
                rango.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; //Alineamos horizontalmente
                rango.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;  //Alineamos verticalmente
                rango.Style.Font.FontSize = 15; //Indicamos el tamaño de la fuente
                rango.Style.Fill.BackgroundColor = XLColor.AliceBlue; //Indicamos el color de background
                worksheet.Columns(1, 8).AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte Radios "+DateTime.Now.ToString()+".xlsx");
                }
            }
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
