namespace Laboratorio.Models
{
    public class Usuario
    {
        public int IdUsuarios { get; set; }
        public string Email { get; set; } = "";
        public string Contrasena { get; set; } = "";
        public string Estado { get; set; } = "";
        public string Rol { get; set; } = "";
    }
}