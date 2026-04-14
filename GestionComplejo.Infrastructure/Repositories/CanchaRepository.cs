using GestionComplejo.Domain.Entities;
using GestionComplejo.Domain.Interfaces;
using GestionComplejo.Infrastructure.Context;

namespace GestionComplejo.Infrastructure.Repositories
{
    public class CanchaRepository : GenericRepository<Cancha>, ICanchaRepository
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

        public CanchaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override List<Cancha> GetAll()
        {
            return _canchas.ToList();
        }

        public override Cancha? GetById(Guid id)
        {
            var cancha = _canchas.FirstOrDefault(c => c.Id == id);
            return cancha;
        }

        public override Cancha Add(Cancha entity)
        {
            _canchas.Add(entity);
            return entity;
        }

        public override void Update(Cancha entity)
        {
            var index = _canchas.FindIndex(c => c.Id == entity.Id);
            if (index != -1)
            {
                _canchas[index] = entity;
            }
        }

        public override void Delete(Guid id)
        {
            var cancha = _canchas.FirstOrDefault(c => c.Id == id);
            if (cancha != null)
            {
                _canchas.Remove(cancha);
            }
        }
    }
}
