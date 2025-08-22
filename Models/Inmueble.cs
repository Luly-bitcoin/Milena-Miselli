namespace Laboratorio.Models
{
    public class Inmueble
    {
        public int IdInmuebles { get; set; }
        public int IdPropietario { get; set; }
        public string uso { get; set; }
        public string tipo { get; set; }
        public int ambientes { get; set; }
        public string direccion { get; set; }
        public string coordenadas { get; set; }
        public decimal precio { get; set; }
        public string estado { get; set; }
    }
}