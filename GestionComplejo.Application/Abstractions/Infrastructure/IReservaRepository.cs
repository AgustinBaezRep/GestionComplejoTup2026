using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Abstractions.Infrastructure
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        Task<bool> ExisteReservaEnHorarioAsync(Guid canchaId, DateTime inicio, DateTime fin);
    }
}
