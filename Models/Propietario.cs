using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratorio.Models
{
    public class Propietario
    {
        [Key]
        [Column("idPropietario")]
        public int IdPropietario { get; set; }
        
        [Column("dni")]
        public string Dni { get; set; } = "";
        
        [Column("nombre")]
        public string Nombre { get; set; } = "";
        
        [Column("apellido")]
        public string Apellido { get; set; } = "";
        
        [Column("direccion")]
        public string? Direccion { get; set; }
        
        [Column("telefono")]
        public string? Telefono { get; set; }
    }
}