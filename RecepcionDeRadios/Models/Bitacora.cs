using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Models
{
    public class Bitacora
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Acción")]
        public string accion { get; set; }
        [DisplayName("Tabla modificada")]
        public string Tabla { get; set; }
        [DisplayName("Propiedad modificada")]
        public string PropertyModified { get; set; }
        [DisplayName("Clave del registro")]
        public string PrimaryKeyValue { get; set; }
        [DisplayName("Valor Anterior")]
        public string OldValue { get; set; }
        [DisplayName("Nuevo Valor")]
        public string NewValue { get; set; }
        [DisplayName("Usuario que modifica")]
        public string IdUsuario { get; set; }
        [DisplayName("Fecha de modificación")]
        public DateTime Fecha { get; set; }
    }
}