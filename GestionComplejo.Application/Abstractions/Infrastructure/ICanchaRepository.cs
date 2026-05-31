using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Abstractions.Infrastructure
{
    public interface ICanchaRepository : IBaseRepository<Cancha>
    {
        Task<Cancha?> AsociarServiciosAsync(Guid canchaId, List<Guid> servicioIds);
        Task<Cancha?> AsociarVestuarioAsync(Guid canchaId, Guid vestuarioId);
    }
}
