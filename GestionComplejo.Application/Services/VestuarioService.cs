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

        public List<VestuarioResponse> GetAll()
        {
            return _vestuarioRepository
                .GetAll()
                .Select(x => x.ToVestuarioResponse())
                .ToList();
        }

        public VestuarioResponse GetById(Guid id)
        {
            var vestuario = _vestuarioRepository.GetById(id);

            if (vestuario == null)
                throw new NotFoundException($"No se encontró un vestuario con id '{id}'.");

            return vestuario.ToVestuarioResponse();
        }

        public VestuarioResponse Create(VestuarioRequest request)
        {
            var newVestuario = request.ToVestuario();
            _vestuarioRepository.Add(newVestuario);
            return newVestuario.ToVestuarioResponse();
        }

        public void Update(VestuarioRequest request, Guid id)
        {
            var vestuario = _vestuarioRepository.GetById(id);

            if (vestuario == null)
                throw new NotFoundException($"No se encontró un vestuario con id '{id}'.");

            vestuario.NumeroVestuarios = request.NumeroVestuarios;
            vestuario.TieneDuchas = request.TieneDuchas;
            vestuario.Capacidad = request.Capacidad;
            vestuario.CanchaId = request.CanchaId;

            _vestuarioRepository.Update(vestuario);
        }

        public void Delete(Guid id)
        {
            var vestuario = _vestuarioRepository.GetById(id);

            if (vestuario == null)
                throw new NotFoundException($"No se encontró un vestuario con id '{id}'.");

            _vestuarioRepository.Delete(id);
        }
    }
}
