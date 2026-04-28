using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Abstractions
{
    public interface ICanchaService
    {
        List<CanchaResponse> GetAll();
        CanchaResponse? GetById(Guid id);
        CanchaResponse Create(CanchaRequest cancha);
        bool Update(CanchaRequest cancha, Guid id);
        bool Delete(Guid id);
        CanchaResponse? AsociarServicios(Guid canchaId, List<Guid> servicioIds);
        CanchaResponse? AsociarVestuario(Guid canchaId, Guid vestuarioId);
    }
}
