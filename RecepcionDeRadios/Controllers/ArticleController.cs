using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace RecepcionDeRadios.Controllers
{
    public class ArticleController : Controller
    {

        // GET: Article/Details/5
        public JsonResult Details(string id)
        {
            var article = "";
            using (var client = new HttpClient())
            {
                var host = Environment.GetEnvironmentVariable("EMPLEADOS_HOST");
                var port = Environment.GetEnvironmentVariable("EMPLEADOS_PORT");
                var uri = $"http://{host}:{port}/api/articulos/";
                client.BaseAddress = new Uri(uri);
                var responseTask = client.GetAsync(id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    article = readTask.Result;
                }
            }
            return Json(article);
        }

    }
}
