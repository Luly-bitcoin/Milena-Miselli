using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratorio.Models
{
    public class Inmueble
    {
        [Key]
        [Column("idInmuebles")]
        public int IdInmuebles { get; set; }

        [Required]
        [Column("idPropietario")]
        public int IdPropietario { get; set; }

        [Required]
        [Column("uso")]
        public string Uso { get; set; } = "";

        [Required]
        [Column("tipo")]
        public int Tipo { get; set; }

        [Required]
        [Column("ambientes")]
        public int Ambientes { get; set; }

        [Required]
        [Column("direccion")]
        public string Direccion { get; set; } = "";

        [Column("coordenadas")]
        public string? Coordenadas { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Required]
        [Column("estado")]
        public string Estado { get; set; } = "";

        // Relaciones (opcional, mejora el display)
        [ForeignKey("IdPropietario")]
        public Propietario? Propietario { get; set; }

        [ForeignKey("Tipo")]
        public TipoInmueble? TipoInmueble { get; set; }
    }
}
