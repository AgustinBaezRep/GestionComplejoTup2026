using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Mapper;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;
using GestionComplejo.Domain.Interfaces;

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
            var canchas = _canchaRepository.GetAll();

            return canchas
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Capacidad)
                .Select(x => x.ToCanchaResponse())
                .ToList();
        }

        public CanchaResponse? GetById(Guid id)
        {
            var cancha = _canchaRepository.GetById(id);

            if (cancha == null || cancha.IsDeleted) 
                return null;

            return cancha.ToCanchaResponse();
        }

        public CanchaResponse Create(CanchaRequest cancha)
        {
            var newCancha = cancha.ToCancha();

            _canchaRepository.Add(newCancha);

            return newCancha.ToCanchaResponse();
        }

        public bool Delete(Guid id)
        {
            var canchaToDelete = _canchaRepository.GetById(id);

            if (canchaToDelete == null || canchaToDelete.IsDeleted)
                return false;

            canchaToDelete.IsDeleted = true;
            _canchaRepository.Update(canchaToDelete);

            return true;
        }

        public bool Update(CanchaRequest cancha, Guid id)
        {
            var canchaToUpdate = _canchaRepository.GetById(id);

            if (canchaToUpdate == null || canchaToUpdate.IsDeleted) 
                return false;
            
            canchaToUpdate.Nombre = cancha.Nombre;
            canchaToUpdate.Deporte = cancha.Deporte;
            canchaToUpdate.Capacidad = cancha.Capacidad;
            canchaToUpdate.Precio = cancha.Precio;

            _canchaRepository.Update(canchaToUpdate);

            return true;
        }
    }
}
