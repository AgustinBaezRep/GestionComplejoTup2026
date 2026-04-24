using GestionComplejo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionComplejo.Infrastructure.Persistance
{
    public class GestionComplejoDbContext : DbContext
    {
        public DbSet<Cancha> Canchas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Vestuario> Vestuarios { get; set; }
        public DbSet<Servicio> Servicios { get; set; }

        public GestionComplejoDbContext(DbContextOptions<GestionComplejoDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().UseTpcMappingStrategy();

            modelBuilder.Entity<Cancha>()
                .HasMany(c => c.Servicios)
                .WithMany(s => s.Canchas)
                .UsingEntity("CanchaServicio");
        }
    }
}
