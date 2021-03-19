using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int ReceipArticleDetailID { get; set; }
        public string Content { get; set; }
        public virtual ReceipArticleDetail ReceipArticleDetail { get; set; }
    }
}