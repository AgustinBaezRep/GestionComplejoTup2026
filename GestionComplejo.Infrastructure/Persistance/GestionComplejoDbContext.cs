using GestionComplejo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionComplejo.Infrastructure.Persistance
{
    public class GestionComplejoDbContext : DbContext
    {
        public DbSet<Cancha> Canchas { get; set; }

        public GestionComplejoDbContext(DbContextOptions<GestionComplejoDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
