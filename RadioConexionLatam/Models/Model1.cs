using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web.UI.WebControls;

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
        public virtual DbSet<Imagenes> Imagenes { get; set; }
        public virtual DbSet<Videos> Videos { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Carrusel> Carrusel { get; set; }
        public virtual DbSet<DetalleCarrusel> DetalleCarrusel { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anuncios>()
                .Property(e => e.contenido)
                .IsUnicode(false);

            // Mapping for Anuncios foreign keys
            modelBuilder.Entity<Anuncios>()
                .HasOptional(a => a.Carrusel)
                .WithMany()
                .HasForeignKey(a => a.idCarrusel);

            modelBuilder.Entity<Anuncios>()
                .HasOptional(a => a.Imagenes)
                .WithMany(i => i.Anuncios)
                .HasForeignKey(a => a.idImagenPrincipal);

            modelBuilder.Entity<Anuncios>()
                .HasOptional(a => a.Videos)
                .WithMany(v => v.Anuncios)
                .HasForeignKey(a => a.idVideoPrincipal);

            modelBuilder.Entity<Anuncios>()
                .HasOptional(a => a.Usuarios)
                .WithMany(u => u.Anuncios)
                .HasForeignKey(a => a.idUsuario);

            modelBuilder.Entity<Anuncios>()
                .HasOptional(a => a.Categorias)
                .WithMany(c => c.Anuncios)
                .HasForeignKey(a => a.idCategoria);

            // Mapping for Carrusel
            modelBuilder.Entity<Carrusel>()
                .HasMany(c => c.DetalleCarrusel)
                .WithRequired(d => d.Carrusel)
                .HasForeignKey(d => d.idCarrusel);

            // Mapping for DetalleCarrusel
            modelBuilder.Entity<DetalleCarrusel>()
                .Property(d => d.url)
                .IsRequired();

        }
    }
}
