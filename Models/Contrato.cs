using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratorio.Models
{
    public class Contrato
    {
        [Key]
        [Column("idContratos")]
        public int IdContratos { get; set; }
        
        [Column("idInquilino")]
        public int IdInquilino { get; set; }
        
        [Column("idInmueble")]
        public int IdInmueble { get; set; }
        
        [Column("idUsuario_crea")]
        public int IdUsuario_crea { get; set; }
        
        [Column("idUsuario_termina")]
        public int? IdUsuario_termina { get; set; }
        
        [Column("monto_mensual")]
        public decimal Monto_mensual { get; set; }
        
        [Column("fecha_inicio")]
        public DateTime Fecha_inicio { get; set; }
        
        [Column("fecha_fin")]
        public DateTime Fecha_fin { get; set; }
        
        [Column("fecha_fin_original")]
        public DateTime Fecha_fin_original { get; set; }
        
        [Column("fecha_termina_anticipada")]
        public DateTime? Fecha_termina_anticipada { get; set; }
        
        [Column("multa")]
        public decimal? Multa { get; set; }

        // Relaciones de navegación
        [ForeignKey("IdInquilino")]
        public Inquilino? Inquilino { get; set; }

        [ForeignKey("IdInmueble")]
        public Inmueble? Inmueble { get; set; }

        [ForeignKey("IdUsuario_crea")]
        public Usuario? UsuarioCrea { get; set; }

        [ForeignKey("IdUsuario_termina")]
        public Usuario? UsuarioTermina { get; set; }
    }
}