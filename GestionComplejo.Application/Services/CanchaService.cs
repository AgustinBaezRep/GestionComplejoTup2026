using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Services
{
    public class CanchaService : ICanchaService
    {
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
            return _canchas
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Capacidad)
                .Select(x => new CanchaResponse
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Deporte = x.Deporte,
                    Precio = x.Precio
                })
                .ToList();
        }

        public CanchaResponse? GetById(Guid id)
        {
            return _canchas
                .Where(x => x.IsDeleted == false && x.Id == id)
                .Select(x => new CanchaResponse
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Deporte = x.Deporte,
                    Precio = x.Precio
                })
                .FirstOrDefault();
        }

        public CanchaResponse Create(CanchaRequest cancha)
        {
            var newCancha = new Cancha
            {
                Id = Guid.NewGuid(),
                Nombre = cancha.Nombre,
                Deporte = cancha.Deporte,
                Capacidad = cancha.Capacidad,
                Precio = cancha.Precio,
                IsDeleted = false
            };

            _canchas.Add(newCancha);

            return new CanchaResponse
            {
                Id = newCancha.Id,
                Nombre = newCancha.Nombre,
                Precio = newCancha.Precio,
                Deporte = newCancha.Deporte
            };
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }


        public Cancha Update(Cancha cancha)
        {
            throw new NotImplementedException();
        }
    }
}
