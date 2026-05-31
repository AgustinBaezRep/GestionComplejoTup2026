using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface ICanchaService
    {
        Task<List<CanchaResponse>> GetAllAsync();
        Task<CanchaResponse> GetByIdAsync(Guid id);
        Task<CanchaResponse> CreateAsync(CanchaRequest cancha);
        Task UpdateAsync(CanchaRequest cancha, Guid id);
        Task DeleteAsync(Guid id);
        Task<CanchaResponse> AsociarServiciosAsync(Guid canchaId, List<Guid> servicioIds);
        Task<CanchaResponse> AsociarVestuarioAsync(Guid canchaId, Guid vestuarioId);
    }
}
