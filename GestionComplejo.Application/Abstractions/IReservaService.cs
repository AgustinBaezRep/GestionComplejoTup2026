using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface IReservaService
    {
        Task<ReservaResponse> CreateAsync(ReservaRequest request);
    }
}
