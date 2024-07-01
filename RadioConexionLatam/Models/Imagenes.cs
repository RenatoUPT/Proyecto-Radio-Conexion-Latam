namespace RadioConexionLatam.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Imagenes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Imagenes()
        {
            Anuncios = new HashSet<Anuncios>();
        }

        [Key]
        public int idImagen { get; set; }

        [Required]
        [StringLength(255)]
        public string url { get; set; }

        [StringLength(255)]
        public string descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Anuncios> Anuncios { get; set; }
    }
}
