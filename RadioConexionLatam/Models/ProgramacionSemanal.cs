using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadioConexionLatam.Models
{
    [Table("ProgramacionSemanal")]
    public class ProgramacionSemanal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Dia { get; set; }

        [Required]
        [StringLength(50)]
        public string Hora { get; set; }

        [Required]
        [StringLength(100)]
        public string NombrePrograma { get; set; }

        [Required]
        public int idUsuario { get; set; }

        [Required]
        public int idCategoria { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuarios Usuario { get; set; }

        [ForeignKey("idCategoria")]
        public virtual Categorias Categoria { get; set; }
    }
}

