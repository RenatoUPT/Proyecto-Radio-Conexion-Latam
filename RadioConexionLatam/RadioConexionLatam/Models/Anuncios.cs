namespace RadioConexionLatam.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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

        public virtual Categorias Categorias { get; set; }

        public virtual Imagenes Imagenes { get; set; }

        public virtual Usuarios Usuarios { get; set; }

        public virtual Videos Videos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnlacesRelacionados> EnlacesRelacionados { get; set; }
    }
}
