using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Abstractions.Infrastructure
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        bool ExisteReservaEnHorario(Guid canchaId, DateTime inicio, DateTime fin);
    }
}
