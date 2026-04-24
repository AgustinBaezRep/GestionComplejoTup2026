using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface IServicioService
    {
        List<ServicioResponse> GetAll();
        ServicioResponse? GetById(Guid id);
        ServicioResponse Create(ServicioRequest request);
        bool Update(ServicioRequest request, Guid id);
        bool Delete(Guid id);
    }
}
