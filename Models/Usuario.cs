using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratorio.Models
{
    public class Usuario
    {
        [Key]
        [Column("idUsuarios")]
        public int IdUsuarios { get; set; }

        [Column("email")]
        public string Email { get; set; } = "";

        [Column("contrasena")]
        public string Contrasena { get; set; } = "";

        [Column("avatar")]
        public string? Avatar { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = "";

        [Column("rol")]
        public string Rol { get; set; } = "";
    }
}