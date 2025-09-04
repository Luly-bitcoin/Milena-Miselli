using Microsoft.EntityFrameworkCore;

namespace Laboratorio.Models
{
    public class LaboratorioContext : DbContext
    {
        public LaboratorioContext(DbContextOptions<LaboratorioContext> options)
            : base(options)
        {
        }

        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<Inquilino> Inquilinos { get; set; }
        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        // Agregar más DbSets
    }
}
