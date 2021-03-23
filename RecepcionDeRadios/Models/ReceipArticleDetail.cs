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
        [DisplayName("Falla")]
        public string ReportedFailure { get; set; }
        [DisplayName("Estatus")]
        public int Status { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ReceipArticle ReceipArticle { get; set; }

    }
}