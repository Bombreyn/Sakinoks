using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Sakinoks.Models
{
    public partial class VortexContext : DbContext
    {
        public VortexContext()
            : base("name=VortexContext")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Kategoriler> Kategoriler { get; set; }
        public virtual DbSet<siteAyar> siteAyar { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Urun> Urun { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<siteAyar>()
                .Property(e => e.hakkinda_baslik)
                .IsUnicode(false);

            modelBuilder.Entity<siteAyar>()
                .Property(e => e.hakkinda_yazi)
                .IsUnicode(false);

            modelBuilder.Entity<siteAyar>()
                .Property(e => e.harita)
                .IsUnicode(false);

            modelBuilder.Entity<siteAyar>()
                .Property(e => e.keywords)
                .IsUnicode(false);

            modelBuilder.Entity<siteAyar>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<siteAyar>()
                .Property(e => e._abstract)
                .IsUnicode(false);

            modelBuilder.Entity<Urun>()
                .Property(e => e.aciklama)
                .IsUnicode(false);
        }
    }
}
