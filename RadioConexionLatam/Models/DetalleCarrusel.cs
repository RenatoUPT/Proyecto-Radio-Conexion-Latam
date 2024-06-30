using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RadioConexionLatam.Models
{
    [Table("DetalleCarrusel")]
    public class DetalleCarrusel
    {
        [Key]
        public int idDetalleCarrusel { get; set; }

        [Required]
        [StringLength(255)]
        public string url { get; set; } // URL for the image in the carousel

        public int idCarrusel { get; set; }

        [ForeignKey("idCarrusel")]
        public virtual Carrusel Carrusel { get; set; }
    }
}