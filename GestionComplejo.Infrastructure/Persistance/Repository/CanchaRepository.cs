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

        public override List<Cancha> GetAll()
        {
            return _context.Canchas
                .Include(c => c.Servicios)
                .Include(c => c.Vestuario)
                .Where(c => !c.IsDeleted)
                .ToList();
        }

        public override Cancha? GetById(Guid id)
        {
            return _context.Canchas
                .Include(c => c.Servicios)
                .Include(c => c.Vestuario)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }

        public Cancha? AsociarServicios(Guid canchaId, List<Guid> servicioIds)
        {
            var cancha = _context.Canchas
                .Include(c => c.Servicios)
                .FirstOrDefault(c => c.Id == canchaId && !c.IsDeleted);

            if (cancha == null) return null;

            var servicios = _context.Servicios
                .Where(s => servicioIds.Contains(s.Id) && !s.IsDeleted)
                .ToList();

            foreach (var servicio in servicios)
            {
                if (!cancha.Servicios.Any(s => s.Id == servicio.Id))
                    cancha.Servicios.Add(servicio);
            }

            SaveChanges();
            return cancha;
        }

        public Cancha? AsociarVestuario(Guid canchaId, Guid vestuarioId)
        {
            var cancha = _context.Canchas
                .FirstOrDefault(c => c.Id == canchaId && !c.IsDeleted);

            if (cancha == null) return null;

            var vestuario = _context.Vestuarios
                .FirstOrDefault(v => v.Id == vestuarioId && !v.IsDeleted);

            if (vestuario == null) return null;

            vestuario.CanchaId = canchaId;
            vestuario.UpdatedDateTime = DateTime.UtcNow;

            SaveChanges();

            return GetById(canchaId);
        }
    }
}
