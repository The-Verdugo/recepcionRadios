using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.Models
{
    public class UserRolesMapping
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public virtual Role Roles { get; set; }
        public virtual User User { get; set; }
    }
}