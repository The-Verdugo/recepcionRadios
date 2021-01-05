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
            var user = new User{ID="1",Name="Ruben Hernandez Puerta",Username="OTH9503",Active=1 };
            context.SaveChanges();
            var roles = new List<Role>
            {
                new Role{Id=1,RoleName="Administrador"},
                new Role{Id=2,RoleName="Tecnico"},
            };
            roles.ForEach(r => context.Roles.Add(r));
            context.SaveChanges();
            base.Seed(context);
        }
    }
}