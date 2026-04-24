using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface IReservaService
    {
        ReservaResponse? Create(ReservaRequest request);
    }
}
