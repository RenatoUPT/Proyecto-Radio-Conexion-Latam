namespace RadioConexionLatam.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EnlacesRelacionados
    {
        [Key]
        public int idEnlace { get; set; }

        public int idAnuncio { get; set; }

        [Required]
        [StringLength(255)]
        public string url { get; set; }

        [StringLength(255)]
        public string descripcion { get; set; }

        public virtual Anuncios Anuncios { get; set; }
    }
}
