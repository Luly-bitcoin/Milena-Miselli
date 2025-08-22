namespace Laboratorio.Models
{
    public class Contrato
    {
        public int IdContratos { get; set; }
        public int IdInmueble { get; set; }
        public int IdInquilino { get; set; }
        public int IdUsuario_crea { get; set; }
        public int IdUsuario_termina { get; set; }
        public decimal monto_mensual { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string fecha_fin_original { get; set; }
        public string fecha_termina_anticipada { get; set; }
        public decimal multa { get; set; }
    }
}