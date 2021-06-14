using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Usuario que recibe")]
        public string usuarioRecibe { get; set; }
        [DisplayName("Usuario que entrega")]
        public string usuarioEntrega { get; set; }
        [Required]
        [DisplayName("Colaborador que entrega")]
        public string empleadoEntrega { get; set; }
        [DisplayName("Colaborador que recibe")]
        public string empleadoRecibe { get; set; }
        [DisplayName("Fecha de recepción")]
        public DateTime fechaRecibido { get; set; }
        [DisplayName("Fecha de entrega")]
        public DateTime? fechaEntregado { get; set; }
        public virtual ICollection<ReceipArticleDetail> ReceipArticleDetails { get; set; }
    }
}