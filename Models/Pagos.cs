using System;

namespace Laboratorio.Models
{
    public class Pago
    {
        public int idPagos { get; set; }
        public int idContrato { get; set; }
        public int id_usuario_crea { get; set; }
        public int? id_usuario_anula { get; set; }
        public int numero_pago { get; set; }
        public DateTime fecha_pago { get; set; }
        public decimal importe { get; set; }
        public string concepto { get; set; }
        public string estado { get; set; }
    }
}