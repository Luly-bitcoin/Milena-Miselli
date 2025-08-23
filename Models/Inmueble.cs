using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratorio.Models
{
    public class Inmueble
    {
        [Key]
        [Column("idInmuebles")]
        public int IdInmuebles { get; set; }
        
        [Column("idPropietario")]
        public int IdPropietario { get; set; }
        
        [Column("uso")]
        public string Uso { get; set; } = "";
        
        [Column("tipo")]
        public int Tipo { get; set; }
        
        [Column("ambientes")]
        public int Ambientes { get; set; }
        
        [Column("direccion")]
        public string Direccion { get; set; } = "";
        
        [Column("coordenadas")]
        public string? Coordenadas { get; set; }
        
        [Column("precio")]
        public decimal Precio { get; set; }
        
        [Column("estado")]
        public string Estado { get; set; } = "";
    }
}