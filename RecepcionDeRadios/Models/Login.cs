using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Usuario es requerido")]
        public String User { get; set; }
        [Required(ErrorMessage = "Password es requerido")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}