using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Application.Exceptions;
using GestionComplejo.Application.Mapper;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ICanchaRepository _canchaRepository;
        private readonly IClimaService _climaService;

        public ReservaService(
            IReservaRepository reservaRepository,
            ICanchaRepository canchaRepository,
            IClimaService climaService)
        {
            _reservaRepository = reservaRepository;
            _canchaRepository = canchaRepository;
            _climaService = climaService;
        }

        public async Task<ReservaResponse> CreateAsync(ReservaRequest request)
        {
            var cancha = await _canchaRepository.GetByIdAsync(request.CanchaId);

            if (cancha == null)
                throw new NotFoundException($"No se encontró una cancha con id '{request.CanchaId}'.");

            var llueveEnEseHorario = await _climaService.EsLluviosoAsync(request.FechaInicio);
            if (llueveEnEseHorario)
                throw new ConflictException("No se puede reservar: se esperan lluvias en ese horario.");

            var fechaFin = request.FechaInicio.AddHours(1);

            if (await _reservaRepository.ExisteReservaEnHorarioAsync(request.CanchaId, request.FechaInicio, fechaFin))
                throw new ConflictException("La cancha ya tiene una reserva en ese horario.");

            var reserva = request.ToReserva(fechaFin, cancha.Precio);
            await _reservaRepository.AddAsync(reserva);

            return reserva.ToReservaResponse();
        }
    }
}
