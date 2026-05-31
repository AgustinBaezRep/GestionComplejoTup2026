using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface IVestuarioService
    {
        Task<List<VestuarioResponse>> GetAllAsync();
        Task<VestuarioResponse> GetByIdAsync(Guid id);
        Task<VestuarioResponse> CreateAsync(VestuarioRequest request);
        Task UpdateAsync(VestuarioRequest request, Guid id);
        Task DeleteAsync(Guid id);
    }
}
