using Microsoft.EntityFrameworkCore;

namespace Laboratorio.Models
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }
        
        // Entidades
        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Inquilino> Inquilinos { get; set; }
        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoInmueble> TiposInmueble { get; set; }
    }
}