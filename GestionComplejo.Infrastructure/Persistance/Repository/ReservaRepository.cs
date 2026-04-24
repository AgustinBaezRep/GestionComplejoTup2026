using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Infrastructure.Persistance.Repository
{
    public class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        public ReservaRepository(GestionComplejoDbContext context) : base(context)
        {
        }

        public bool ExisteReservaEnHorario(Guid canchaId, DateTime inicio, DateTime fin)
        {
            return _dbSet.Any(r =>
                !r.IsDeleted &&
                r.CanchaId == canchaId &&
                (r.Estado == "Pendiente" || r.Estado == "Confirmada") &&
                r.FechaInicio < fin &&
                r.FechaFin > inicio);
        }
    }
}


// nueva    [-------]
// existente         [-------]
// el horario de fin de la nueva, es mas chico que el horario de inicio de la existente
// r.FechaInicio > fin


// nueva              [-------]
// existente [-------]
// el horario de inicio de la nueva, es mas grande que el horario de fin de la existente
// r.FechaFin < inicio