using System;
using System.Collections.Generic;
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
        public string ArticleID { get; set; }
        public string ReportedFailure { get; set; }
        public int Status { get; set; }
        public virtual ReceipArticle ReceipArticle { get; set; }

    }
}