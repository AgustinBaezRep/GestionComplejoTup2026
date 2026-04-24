using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Application.Mapper;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ICanchaRepository _canchaRepository;

        public ReservaService(IReservaRepository reservaRepository, ICanchaRepository canchaRepository)
        {
            _reservaRepository = reservaRepository;
            _canchaRepository = canchaRepository;
        }

        public ReservaResponse? Create(ReservaRequest request)
        {
            var fechaFin = request.FechaInicio.AddHours(1);

            if (_reservaRepository.ExisteReservaEnHorario(request.CanchaId, request.FechaInicio, fechaFin))
                return null;

            var cancha = _canchaRepository.GetById(request.CanchaId);
            if (cancha == null)
                return null;

            var reserva = request.ToReserva(fechaFin, cancha.Precio);
            _reservaRepository.Add(reserva);

            return reserva.ToReservaResponse();
        }
    }
}
