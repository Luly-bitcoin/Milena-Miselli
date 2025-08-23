using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratorio.Models
{
    [Table("inquilinos")]
    public class Inquilino
    {
        [Key]
        [Column("idInquilinos")]
        public int IdInquilino { get; set; }
        
        [Column("dni")]
        public string Dni { get; set; } = "";
        
        [Column("nombre")]
        public string Nombre { get; set; } = "";
        
        [Column("apellido")]
        public string Apellido { get; set; } = "";
        
        [Column("telefono")]
        public string? Telefono { get; set; }
    }
}