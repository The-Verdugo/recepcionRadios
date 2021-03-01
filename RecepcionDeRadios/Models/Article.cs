using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Models
{
    public class Article
    {
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NoSerie { get; set; }
        public string NoInterno { get; set; }
    }
}