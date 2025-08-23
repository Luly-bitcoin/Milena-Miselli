namespace Laboratorio.Models
{
    public class Inmueble
    {
        public int IdInmuebles { get; set; }
        public int IdPropietario { get; set; }
        public string Uso { get; set; } = "";
        public string Tipo { get; set; } = "";
        public int Ambientes { get; set; }
        public string Direccion { get; set; } = "";
        public string? Coordenadas { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; } = "";
    }
}