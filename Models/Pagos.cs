using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratorio.Models
{
    public class Pagos
    {
        [Key]
        [Column("idPagos")]
        public int IdPagos { get; set; }
        
        [Column("idContrato")]
        public int IdContrato { get; set; }
        
        [Column("id_usuario_crea")]
        public int Id_usuario_crea { get; set; }
        
        [Column("id_usuario_anula")]
        public int? Id_usuario_anula { get; set; }
        
        [Column("numero_pago")]
        public int Numero_pago { get; set; }
        
        [Column("fecha_pago")]
        public DateTime Fecha_pago { get; set; }
        
        [Column("importe")]
        public decimal Importe { get; set; }
        
        [Column("concepto")]
        public string Concepto { get; set; } = "";
        
        [Column("estado")]
        public string Estado { get; set; } = "";
    }
}