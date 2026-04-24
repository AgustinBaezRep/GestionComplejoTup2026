using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Mapper
{
    public static class ServicioMapper
    {
        public static ServicioResponse ToServicioResponse(this Servicio servicio)
        {
            return new ServicioResponse
            {
                Id = servicio.Id,
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                CostoAdicional = servicio.CostoAdicional
            };
        }

        public static Servicio ToServicio(this ServicioRequest request)
        {
            return new Servicio
            {
                Id = Guid.NewGuid(),
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                CostoAdicional = request.CostoAdicional,
                IsDeleted = false
            };
        }
    }
}
