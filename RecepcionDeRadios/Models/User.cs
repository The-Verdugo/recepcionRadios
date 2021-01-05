using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Models
{
    public class User
    {
        public String ID { get; set; }
        public String Name { get; set; }
        public String Username { get; set; }

        [DataType(DataType.Password)]
        public String Password { get; set; }
        public int Level { get; set; }
        public int Active { get; set; }
        public virtual ICollection<UserRolesMapping> UserRolesMapping { get; set; }
    }
}