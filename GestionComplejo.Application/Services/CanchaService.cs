using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Application.Mapper;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Services
{
    public class CanchaService : ICanchaService
    {
        private readonly ICanchaRepository _canchaRepository;

        public CanchaService(ICanchaRepository canchaRepository)
        {
            _canchaRepository = canchaRepository;
        }

        public List<CanchaResponse> GetAll()
        {
            return _canchaRepository
                .GetAll()
                .OrderBy(x => x.Capacidad)
                .Select(x => x.ToCanchaResponse())
                .ToList();
        }

        public CanchaResponse? GetById(Guid id)
        {
            return _canchaRepository.GetById(id)?.ToCanchaResponse();
        }

        public CanchaResponse Create(CanchaRequest cancha)
        {
            var newCancha = cancha.ToCancha();

            _canchaRepository.Add(newCancha);

            return newCancha.ToCanchaResponse();
        }

        public bool Delete(Guid id)
        {
            _canchaRepository.Delete(id);

            return true;
        }


        public bool Update(CanchaRequest cancha, Guid id)
        {
            var canchaToUpdate = _canchaRepository.GetById(id);

            if (canchaToUpdate == null)
                return false;

            canchaToUpdate.Nombre = cancha.Nombre;
            canchaToUpdate.Deporte = cancha.Deporte;
            canchaToUpdate.Capacidad = cancha.Capacidad;
            canchaToUpdate.Precio = cancha.Precio;

            _canchaRepository.Update(canchaToUpdate);

            return true;
        }

        public CanchaResponse? AsociarServicios(Guid canchaId, List<Guid> servicioIds)
        {
            var cancha = _canchaRepository.AsociarServicios(canchaId, servicioIds);
            return cancha?.ToCanchaResponse();
        }

        public CanchaResponse? AsociarVestuario(Guid canchaId, Guid vestuarioId)
        {
            var cancha = _canchaRepository.AsociarVestuario(canchaId, vestuarioId);
            return cancha?.ToCanchaResponse();
        }
    }
}
