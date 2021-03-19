using RecepcionDeRadios.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace RecepcionDeRadios.DAL
{
    public class RecepcionDeRadiosContext : DbContext
    {
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Comment> Comments { get; set; }
    }
}