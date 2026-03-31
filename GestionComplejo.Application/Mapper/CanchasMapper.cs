using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Mapper
{
    public static class CanchasMapper
    {
        public static CanchaResponse ToCanchaResponse(this Cancha cancha)
        {
            return new CanchaResponse
            {
                Id = cancha.Id,
                Nombre = cancha.Nombre,
                Deporte = cancha.Deporte,
                Precio = cancha.Precio
            };
        }

        public static Cancha ToCancha(this CanchaRequest canchaRequest)
        {
            return new Cancha
            {
                Id = Guid.NewGuid(),
                Nombre = canchaRequest.Nombre,
                Deporte = canchaRequest.Deporte,
                Capacidad = canchaRequest.Capacidad,
                Precio = canchaRequest.Precio,
                IsDeleted = false
            };
        }
    }
}
