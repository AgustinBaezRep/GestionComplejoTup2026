using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface IServicioService
    {
        Task<List<ServicioResponse>> GetAllAsync();
        Task<ServicioResponse> GetByIdAsync(Guid id);
        Task<ServicioResponse> CreateAsync(ServicioRequest request);
        Task UpdateAsync(ServicioRequest request, Guid id);
        Task DeleteAsync(Guid id);
    }
}
