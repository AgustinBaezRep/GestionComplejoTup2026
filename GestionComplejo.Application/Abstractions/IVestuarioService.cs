using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface IVestuarioService
    {
        List<VestuarioResponse> GetAll();
        VestuarioResponse? GetById(Guid id);
        VestuarioResponse Create(VestuarioRequest request);
        bool Update(VestuarioRequest request, Guid id);
        bool Delete(Guid id);
    }
}
