using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Infrastructure.Persistance.Repository
{
    public class ServicioRepository : BaseRepository<Servicio>, IServicioRepository
    {
        public ServicioRepository(GestionComplejoDbContext context) : base(context)
        {
        }
    }
}
