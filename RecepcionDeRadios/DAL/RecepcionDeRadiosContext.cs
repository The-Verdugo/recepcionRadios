
using RecepcionDeRadios.Helpers;
using RecepcionDeRadios.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Web;

namespace RecepcionDeRadios.DAL
{
    public class RecepcionDeRadiosContext : DbContext
    {
        private keysGet keysGet = new keysGet();
        private const string secretKey = Crypto.Crypto.KEY;
        public RecepcionDeRadiosContext() : base(Crypto.Crypto.SimpleDecryptWithPassword(Environment.GetEnvironmentVariable("SISRADIOS"), secretKey))
        {
            
        }
        //public DbSet<Seller> Sellers { get; set; }
        //public DbSet<Ride> Rides { get; set; }
        public virtual DbSet<User> Users { get; set; }
        //public DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRolesMapping> UserRolesMapping { get; set; }
        public virtual DbSet<ReceipArticle> ReceipArticles { get; set; }
        public virtual DbSet<ReceipArticleDetail> ReceipArticleDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Bitacora> ChangeLogs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }



        public override int SaveChanges()
        {
            #region modificaciones a la bitacora

            string currentValue = "";
            var modifiedChanges = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).ToList();
            var now = DateTime.UtcNow;

            foreach (var change in modifiedChanges)
            {
                var entityName = change.Entity.GetType().Name;
                var primaryKey = GetPrimaryKeyValue(change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.GetDatabaseValues().GetValue<object>(prop);
                    if (change.CurrentValues[prop] != null)
                    {
                        currentValue = change.CurrentValues[prop].ToString();
                    }
                    else {
                        currentValue = null;
                    }
                    if (originalValue == null)
                    {
                        originalValue = "";

                        if (currentValue == null)
                        {
                            currentValue = "";
                            if (originalValue.ToString() != currentValue)
                            {
                                Bitacora log = new Bitacora()
                                {
                                    accion = "Modificación",
                                    Tabla = entityName,
                                    PrimaryKeyValue = primaryKey.ToString(),
                                    PropertyModified = prop,
                                    OldValue = originalValue.ToString(),
                                    NewValue = currentValue,
                                    Fecha = now,
                                    IdUsuario = keysGet.keygeyuser()
                                };
                                ChangeLogs.Add(log);
                            }
                        }
                        else {
                            if (originalValue.ToString() != currentValue)
                            {
                                Bitacora log = new Bitacora()
                                {
                                    accion = "Modificación",
                                    Tabla = entityName,
                                    PrimaryKeyValue = primaryKey.ToString(),
                                    PropertyModified = prop,
                                    OldValue = originalValue.ToString(),
                                    NewValue = currentValue,
                                    Fecha = now,
                                    IdUsuario = keysGet.keygeyuser()
                                };
                                ChangeLogs.Add(log);
                            }
                        }

                    }
                    else if (currentValue == null)
                    {
                        currentValue = "";
                        if (originalValue.ToString() != currentValue)
                        {
                            Bitacora log = new Bitacora()
                            {
                                accion = "Modificación",
                                Tabla = entityName,
                                PrimaryKeyValue = primaryKey.ToString(),
                                PropertyModified = prop,
                                OldValue = originalValue.ToString(),
                                NewValue = currentValue,
                                Fecha = now,
                                IdUsuario = keysGet.keygeyuser()
                            };
                            ChangeLogs.Add(log);
                        }
                    }
                    else {
                        if (originalValue.ToString() != currentValue)
                        {
                            Bitacora log = new Bitacora()
                            {
                                accion = "Modificación",
                                Tabla = entityName,
                                PrimaryKeyValue = primaryKey.ToString(),
                                PropertyModified = prop,
                                OldValue = originalValue.ToString(),
                                NewValue = currentValue,
                                Fecha = now,
                                IdUsuario = keysGet.keygeyuser()
                            };
                            ChangeLogs.Add(log);
                        }
                    } 
                }
            }
            #endregion



            return base.SaveChanges();
        }

    }
}