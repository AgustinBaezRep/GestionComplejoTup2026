using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Mapper
{
    public static class ReservaMapper
    {
        public static Reserva ToReserva(this ReservaRequest request, DateTime fechaFin, double precioTotal)
        {
            return new Reserva
            {
                Id = Guid.NewGuid(),
                ClienteId = request.ClienteId,
                CanchaId = request.CanchaId,
                FechaInicio = request.FechaInicio,
                FechaFin = fechaFin,
                Estado = "Pendiente",
                PrecioTotal = precioTotal,
                IsDeleted = false
            };
        }

        public static ReservaResponse ToReservaResponse(this Reserva reserva)
        {
            return new ReservaResponse
            {
                Id = reserva.Id,
                ClienteId = reserva.ClienteId,
                CanchaId = reserva.CanchaId,
                FechaInicio = reserva.FechaInicio,
                FechaFin = reserva.FechaFin,
                Estado = reserva.Estado,
                PrecioTotal = reserva.PrecioTotal
            };
        }
    }
}
