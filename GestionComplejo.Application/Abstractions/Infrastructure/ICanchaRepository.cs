using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Abstractions.Infrastructure
{
    public interface ICanchaRepository : IBaseRepository<Cancha>
    {
        Cancha? AsociarServicios(Guid canchaId, List<Guid> servicioIds);
        Cancha? AsociarVestuario(Guid canchaId, Guid vestuarioId);
    }
}
