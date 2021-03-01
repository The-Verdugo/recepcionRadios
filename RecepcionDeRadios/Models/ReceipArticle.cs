using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Models
{
    public class ReceipArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string usuarioRecibe { get; set; }
        public string usuarioEntrega { get; set; }
        public string empleadoEntrega { get; set; }
        public string empleadoRecibe { get; set; }
        public DateTime fechaRecibido { get; set; }
        public DateTime fechaEntregado { get; set; }
        public virtual ICollection<ReceipArticleDetail> ReceipArticleDetails { get; set; }
    }
}