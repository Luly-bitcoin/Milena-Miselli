using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratorio.Models
{
    public class TipoInmueble
    {
        [Key]
        [Column("idTipo")]
        public int IdTipo { get; set; }
        
        [Column("descripcion")]
        public string Descripcion { get; set; } = "";
    }
}