using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Application.Exceptions;
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

        public CanchaResponse GetById(Guid id)
        {
            var cancha = _canchaRepository.GetById(id);

            if (cancha == null)
                throw new NotFoundException($"No se encontró una cancha con id '{id}'.");

            return cancha.ToCanchaResponse();
        }

        public CanchaResponse Create(CanchaRequest cancha)
        {
            var newCancha = cancha.ToCancha();
            _canchaRepository.Add(newCancha);
            return newCancha.ToCanchaResponse();
        }

        public void Delete(Guid id)
        {
            var cancha = _canchaRepository.GetById(id);

            if (cancha == null)
                throw new NotFoundException($"No se encontró una cancha con id '{id}'.");

            _canchaRepository.Delete(id);
        }

        public void Update(CanchaRequest cancha, Guid id)
        {
            var canchaToUpdate = _canchaRepository.GetById(id);

            if (canchaToUpdate == null)
                throw new NotFoundException($"No se encontró una cancha con id '{id}'.");

            canchaToUpdate.Nombre = cancha.Nombre;
            canchaToUpdate.Deporte = cancha.Deporte;
            canchaToUpdate.Capacidad = cancha.Capacidad;
            canchaToUpdate.Precio = cancha.Precio;

            _canchaRepository.Update(canchaToUpdate);
        }

        public CanchaResponse AsociarServicios(Guid canchaId, List<Guid> servicioIds)
        {
            var cancha = _canchaRepository.AsociarServicios(canchaId, servicioIds);

            if (cancha == null)
                throw new NotFoundException($"No se encontró una cancha con id '{canchaId}'.");

            return cancha.ToCanchaResponse();
        }

        public CanchaResponse AsociarVestuario(Guid canchaId, Guid vestuarioId)
        {
            var cancha = _canchaRepository.AsociarVestuario(canchaId, vestuarioId);

            if (cancha == null)
                throw new NotFoundException($"No se encontró la cancha o el vestuario especificado.");

            return cancha.ToCanchaResponse();
        }
    }
}
