using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace RadioConexionLatam.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Anuncios> Anuncios { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<EnlacesRelacionados> EnlacesRelacionados { get; set; }
        public virtual DbSet<Imagenes> Imagenes { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Videos> Videos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anuncios>()
                .Property(e => e.contenido)
                .IsUnicode(false);

            modelBuilder.Entity<Anuncios>()
                .HasMany(e => e.EnlacesRelacionados)
                .WithRequired(e => e.Anuncios)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Imagenes>()
                .HasMany(e => e.Anuncios)
                .WithOptional(e => e.Imagenes)
                .HasForeignKey(e => e.idImagenPrincipal);

            modelBuilder.Entity<Videos>()
                .HasMany(e => e.Anuncios)
                .WithOptional(e => e.Videos)
                .HasForeignKey(e => e.idVideoPrincipal);
        }
    }
}
