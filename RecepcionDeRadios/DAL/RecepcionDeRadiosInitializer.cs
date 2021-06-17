using RecepcionDeRadios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.DAL
{
    public class RecepcionDeRadiosInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<RecepcionDeRadiosContext>
    {
        protected override void Seed(RecepcionDeRadiosContext context)
        {
            var user = new User{ID="1",Name="Administrador",Username="admin",Password = BCrypt.Net.BCrypt.HashPassword("admin"),Level=1,Active=1 };
            context.Users.Add(user);
            context.SaveChanges();
            var roles = new List<Role>
            {
                new Role{Id=1,RoleName="Administrador"},
                new Role{Id=2,RoleName="Tecnico"},
            };
            roles.ForEach(r => context.Roles.Add(r));
            var rolemap = new UserRolesMapping { UserId = "1", RoleId = 1 };
            context.UserRolesMapping.Add(rolemap);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}