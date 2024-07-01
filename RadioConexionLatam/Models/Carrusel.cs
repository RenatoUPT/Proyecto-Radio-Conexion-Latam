using System;

using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RadioConexionLatam.Models
{
    [Table("Carrusel")]
    public class Carrusel
    {
        public Carrusel()
        {
            this.DetalleCarrusel = new HashSet<DetalleCarrusel>();
        }

        [Key]
        public int idCarrusel { get; set; }

        [StringLength(100)]
        public string descripcion { get; set; }


        public virtual ICollection<DetalleCarrusel> DetalleCarrusel { get; set; }


        public void AddDetalle(DetalleCarrusel detalle)
        {
            detalle.idCarrusel = this.idCarrusel;
            this.DetalleCarrusel.Add(detalle);
        }
    }


}