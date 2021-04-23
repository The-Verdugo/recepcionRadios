using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Models
{
    public class ReceipArticleDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ReceipArticleID { get; set; }
        [Required]
        [DisplayName("Id del Articulo")]
        public string ArticleID { get; set; }
        [Required]
        [DisplayName("Descripcion")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Falla")]
        public string ReportedFailure { get; set; }
        [DisplayName("Fecha de entrega al usuario")]
        public DateTime? FechaEntregaUsuario { get; set; }
        [DisplayName("Fecha de entrega al proveedor")]
        public DateTime? FechaEntregaProveedor { get; set; }
        [DisplayName("Pase de salida")]
        public string PaseSalida { get; set; }
        [DisplayName("Folio de baja")]
        public string FolioBaja { get; set; }
        [DisplayName("Estatus")]
        public int Status { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ReceipArticle ReceipArticle { get; set; }

    }
}