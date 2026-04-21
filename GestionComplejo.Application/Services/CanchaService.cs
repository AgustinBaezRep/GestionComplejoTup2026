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

        private static readonly List<Cancha> _canchas = new()
        {
            new Cancha
            {
                Id = Guid.NewGuid(),
                Nombre = "Cancha 1",
                Deporte = "Fútbol 5",
                Capacidad = 10,
                Precio = 40000.0,
                IsDeleted = false
            },
            new Cancha
            {
                Id = Guid.NewGuid(),
                Nombre = "Cancha 2",
                Deporte = "Fútbol 7",
                Capacidad = 14,
                Precio = 800000.0,
                IsDeleted = false
            },
            new Cancha
            {
                Id = Guid.NewGuid(),
                Nombre = "Cancha 3",
                Deporte = "Fútbol 7",
                Capacidad = 14,
                Precio = 800000.0,
                IsDeleted = true
            },
            new Cancha
            {
                Id = Guid.NewGuid(),
                Nombre = "Cancha 4",
                Deporte = "Fútbol 7",
                Capacidad = 14,
                Precio = 800000.0,
                IsDeleted = true
            },
            new Cancha
            {
                Id = Guid.NewGuid(),
                Nombre = "Cancha 5",
                Deporte = "Fútbol 7",
                Capacidad = 14,
                Precio = 800000.0,
                IsDeleted = false
            }
        };

        public List<CanchaResponse> GetAll()
        {
            var canchas = _canchaRepository.GetAll();

            return _canchas
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Capacidad)
                .Select(x => x.ToCanchaResponse())
                .ToList();
        }

        public CanchaResponse? GetById(Guid id)
        {
            return _canchas
                .Where(x => x.IsDeleted == false && x.Id == id)
                .Select(x => x.ToCanchaResponse())
                .FirstOrDefault();
        }

        public CanchaResponse Create(CanchaRequest cancha)
        {
            var newCancha = cancha.ToCancha();

            _canchas.Add(newCancha);

            return newCancha.ToCanchaResponse();
        }

        public bool Delete(Guid id)
        {
            var canchaToDelete = _canchas.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (canchaToDelete == null)
                return false;

            canchaToDelete.IsDeleted = true;

            return true;
        }


        public bool Update(CanchaRequest cancha, Guid id)
        {
            var canchaToUpdate = _canchas.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (canchaToUpdate == null) 
                return false;
            
            canchaToUpdate.Nombre = cancha.Nombre;
            canchaToUpdate.Deporte = cancha.Deporte;
            canchaToUpdate.Capacidad = cancha.Capacidad;
            canchaToUpdate.Precio = cancha.Precio;

            return true;
        }
    }
}
