namespace RadioConexionLatam.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Anuncios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Anuncios()
        {
            EnlacesRelacionados = new HashSet<EnlacesRelacionados>();
        }

        [Key]
        public int idAnuncio { get; set; }

        [Required]
        [StringLength(255)]
        public string titulo { get; set; }

        [StringLength(255)]
        public string subtitulo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string contenido { get; set; }

        public DateTime fechaPublicacion { get; set; }

        public int? idUsuario { get; set; }

        public int? idCategoria { get; set; }

        public int? idImagenPrincipal { get; set; }

        public int? idVideoPrincipal { get; set; }

        [StringLength(1)]
        public string estado { get; set; }

        public virtual Categorias Categorias { get; set; }

        public virtual Imagenes Imagenes { get; set; }

        public virtual Usuarios Usuarios { get; set; }

        public virtual Videos Videos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnlacesRelacionados> EnlacesRelacionados { get; set; }


        [NotMapped]
        public string ImagenRuta { get; set; }
        [NotMapped]
        public string VideoUrl { get; set; }


        //Método Listar
        public List<Anuncios> Listar()
        {
            var query = new List<Anuncios>();
            try
            {
                using (var db = new Model1())
                {
                    query = db.Anuncios.ToList();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return query;
        }

        //Metodo Buscar
        public List<Anuncios> Buscar(string Buscar)
        {
            var query = new List<Anuncios>();
            try
            {
                using (var db = new Model1())
                {
                    // Si la búsqueda contiene solo números, busca por idAnuncio, de lo contrario, busca por título
                    if (int.TryParse(Buscar, out int idAnuncio))
                    {
                        query = db.Anuncios
                            .Where(x => x.idAnuncio == idAnuncio)
                            .ToList();
                    }
                    else
                    {
                        query = db.Anuncios
                            .Where(x => x.titulo.Contains(Buscar))
                            .ToList();
                    }

                    // Incluir datos relacionados si es necesario
                    foreach (var anuncio in query)
                    {
                        if (anuncio.idImagenPrincipal.HasValue)
                        {
                            anuncio.Imagenes = db.Imagenes.Find(anuncio.idImagenPrincipal.Value);
                            if (anuncio.Imagenes != null)
                            {
                                anuncio.ImagenRuta = anuncio.Imagenes.url;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepción con más contexto
                throw new ApplicationException("Error al buscar anuncios: " + ex.Message, ex);
            }
            return query;
        }



        //Metodo Guardar
        public void Guardar()
        {

            try
            {
                using (var db = new Model1())
                {
                    if (this.idAnuncio > 0)// por existe en la bd
                    {
                        db.Entry(this).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Entry(this).State = EntityState.Added;
                    }
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
