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
                Precio = cancha.Precio,
                Servicios = cancha.Servicios?.Select(s => s.ToServicioResponse()).ToList() ?? new(),
                Vestuario = cancha.Vestuario != null && !cancha.Vestuario.IsDeleted
                    ? cancha.Vestuario.ToVestuarioResponse()
                    : null
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
