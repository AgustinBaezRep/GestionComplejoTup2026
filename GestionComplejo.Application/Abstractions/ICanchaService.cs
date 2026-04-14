using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Application.Services;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Abstractions
{
    public interface ICanchaService
    {
        List<CanchaResponse> GetAll();
        CanchaResponse? GetById(Guid id);
        CanchaResponse Create(CanchaRequest cancha);
        bool Update(CanchaRequest cancha, Guid id);
        bool Delete(Guid id);
    }
}
