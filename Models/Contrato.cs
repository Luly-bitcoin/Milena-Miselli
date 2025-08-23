using System;

namespace Laboratorio.Models
{
    public class Contrato
    {
        public int IdContratos { get; set; }
        public int IdInquilino { get; set; }
        public int IdInmueble { get; set; }
        public int IdUsuario_crea { get; set; }
        public int? IdUsuario_termina { get; set; }
        public decimal Monto_mensual { get; set; }
        public DateTime Fecha_inicio { get; set; }
        public DateTime Fecha_fin { get; set; }
        public DateTime Fecha_fin_original { get; set; }
        public DateTime? Fecha_termina_anticipada { get; set; }
        public decimal? Multa { get; set; }
    }
}