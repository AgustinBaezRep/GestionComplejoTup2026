using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface IVestuarioService
    {
        List<VestuarioResponse> GetAll();
        VestuarioResponse GetById(Guid id);
        VestuarioResponse Create(VestuarioRequest request);
        void Update(VestuarioRequest request, Guid id);
        void Delete(Guid id);
    }
}
