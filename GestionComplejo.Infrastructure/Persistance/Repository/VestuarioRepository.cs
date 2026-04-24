using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Infrastructure.Persistance.Repository
{
    public class VestuarioRepository : BaseRepository<Vestuario>, IVestuarioRepository
    {
        public VestuarioRepository(GestionComplejoDbContext context) : base(context)
        {
        }
    }
}
