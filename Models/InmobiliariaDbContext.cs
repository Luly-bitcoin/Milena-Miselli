using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratorio.Models
{
    public class InmobiliariaDbContext : DbContext
    {
        public InmobiliariaDbContext(DbContextOptions<InmobiliariaDbContext> options) : base(options)
        {

        }
        
        // Entidades
        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Inquilino> Inquilinos { get; set; }
        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Pagos> Pagos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoInmueble> TiposInmueble { get; set; }
    }
}