using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Application.Exceptions;
using GestionComplejo.Application.Mapper;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Services
{
    public class VestuarioService : IVestuarioService
    {
        private readonly IVestuarioRepository _vestuarioRepository;

        public VestuarioService(IVestuarioRepository vestuarioRepository)
        {
            _vestuarioRepository = vestuarioRepository;
        }

        public async Task<List<VestuarioResponse>> GetAllAsync()
        {
            return (await _vestuarioRepository.GetAllAsync())
                .Select(x => x.ToVestuarioResponse())
                .ToList();
        }

        public async Task<VestuarioResponse> GetByIdAsync(Guid id)
        {
            var vestuario = await _vestuarioRepository.GetByIdAsync(id);

            if (vestuario == null)
                throw new NotFoundException($"No se encontró un vestuario con id '{id}'.");

            return vestuario.ToVestuarioResponse();
        }

        public async Task<VestuarioResponse> CreateAsync(VestuarioRequest request)
        {
            var newVestuario = request.ToVestuario();
            await _vestuarioRepository.AddAsync(newVestuario);
            return newVestuario.ToVestuarioResponse();
        }

        public async Task UpdateAsync(VestuarioRequest request, Guid id)
        {
            var vestuario = await _vestuarioRepository.GetByIdAsync(id);

            if (vestuario == null)
                throw new NotFoundException($"No se encontró un vestuario con id '{id}'.");

            vestuario.NumeroVestuarios = request.NumeroVestuarios;
            vestuario.TieneDuchas = request.TieneDuchas;
            vestuario.Capacidad = request.Capacidad;
            vestuario.CanchaId = request.CanchaId;

            await _vestuarioRepository.UpdateAsync(vestuario);
        }

        public async Task DeleteAsync(Guid id)
        {
            var vestuario = await _vestuarioRepository.GetByIdAsync(id);

            if (vestuario == null)
                throw new NotFoundException($"No se encontró un vestuario con id '{id}'.");

            await _vestuarioRepository.DeleteAsync(id);
        }
    }
}
