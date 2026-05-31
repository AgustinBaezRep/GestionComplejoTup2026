using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionComplejo.Infrastructure.Persistance.Repository
{
    public class CanchaRepository : BaseRepository<Cancha>, ICanchaRepository
    {
        public CanchaRepository(GestionComplejoDbContext context) : base(context)
        {
        }

        public override async Task<List<Cancha>> GetAllAsync()
        {
            return await _context.Canchas
                .Include(c => c.Servicios)
                .Include(c => c.Vestuario)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public override async Task<Cancha?> GetByIdAsync(Guid id)
        {
            return await _context.Canchas
                .Include(c => c.Servicios)
                .Include(c => c.Vestuario)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<Cancha?> AsociarServiciosAsync(Guid canchaId, List<Guid> servicioIds)
        {
            var cancha = await _context.Canchas
                .Include(c => c.Servicios)
                .FirstOrDefaultAsync(c => c.Id == canchaId && !c.IsDeleted);

            if (cancha == null) return null;

            var servicios = await _context.Servicios
                .Where(s => servicioIds.Contains(s.Id) && !s.IsDeleted)
                .ToListAsync();

            foreach (var servicio in servicios)
            {
                if (!cancha.Servicios.Any(s => s.Id == servicio.Id))
                    cancha.Servicios.Add(servicio);
            }

            await SaveChangesAsync();
            return cancha;
        }

        public async Task<Cancha?> AsociarVestuarioAsync(Guid canchaId, Guid vestuarioId)
        {
            var cancha = await _context.Canchas
                .FirstOrDefaultAsync(c => c.Id == canchaId && !c.IsDeleted);

            if (cancha == null) return null;

            var vestuario = await _context.Vestuarios
                .FirstOrDefaultAsync(v => v.Id == vestuarioId && !v.IsDeleted);

            if (vestuario == null) return null;

            vestuario.CanchaId = canchaId;
            vestuario.UpdatedDateTime = DateTime.UtcNow;

            await SaveChangesAsync();

            return await GetByIdAsync(canchaId);
        }
    }
}
