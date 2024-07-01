using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadioConexionLatam.Models
{
    [Table("Eventos")]
    public class Eventos
    {
        [Key]
        public int idEvento { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreEvento { get; set; }

        [StringLength(255)]
        public string descripcion { get; set; }

        [Required]
        public DateTime fechaEvento { get; set; }

        [StringLength(100)]
        public string lugar { get; set; }

        [StringLength(100)]
        public string organizador { get; set; }

        [Required]
        [StringLength(1)]
        public string estado { get; set; }

        [Required]
        public int capacidad { get; set; }

        [Required]
        public int idCategoria { get; set; }
        public byte[] Imagen { get; set; }

        [ForeignKey("idCategoria")]
        public virtual Categorias Categoria { get; set; }

    }
}
