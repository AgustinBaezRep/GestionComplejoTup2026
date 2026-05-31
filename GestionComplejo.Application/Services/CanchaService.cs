using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Application.Exceptions;
using GestionComplejo.Application.Mapper;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Services
{
    public class CanchaService : ICanchaService
    {
        private readonly ICanchaRepository _canchaRepository;

        public CanchaService(ICanchaRepository canchaRepository)
        {
            _canchaRepository = canchaRepository;
        }

        public async Task<List<CanchaResponse>> GetAllAsync()
        {
            return (await _canchaRepository.GetAllAsync())
                .OrderBy(x => x.Capacidad)
                .Select(x => x.ToCanchaResponse())
                .ToList();
        }

        public async Task<CanchaResponse> GetByIdAsync(Guid id)
        {
            var cancha = await _canchaRepository.GetByIdAsync(id);

            if (cancha == null)
                throw new NotFoundException($"No se encontró una cancha con id '{id}'.");

            return cancha.ToCanchaResponse();
        }

        public async Task<CanchaResponse> CreateAsync(CanchaRequest cancha)
        {
            var newCancha = cancha.ToCancha();
            await _canchaRepository.AddAsync(newCancha);
            return newCancha.ToCanchaResponse();
        }

        public async Task DeleteAsync(Guid id)
        {
            var cancha = await _canchaRepository.GetByIdAsync(id);

            if (cancha == null)
                throw new NotFoundException($"No se encontró una cancha con id '{id}'.");

            await _canchaRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(CanchaRequest cancha, Guid id)
        {
            var canchaToUpdate = await _canchaRepository.GetByIdAsync(id);

            if (canchaToUpdate == null)
                throw new NotFoundException($"No se encontró una cancha con id '{id}'.");

            canchaToUpdate.Nombre = cancha.Nombre;
            canchaToUpdate.Deporte = cancha.Deporte;
            canchaToUpdate.Capacidad = cancha.Capacidad;
            canchaToUpdate.Precio = cancha.Precio;

            await _canchaRepository.UpdateAsync(canchaToUpdate);
        }

        public async Task<CanchaResponse> AsociarServiciosAsync(Guid canchaId, List<Guid> servicioIds)
        {
            var cancha = await _canchaRepository.AsociarServiciosAsync(canchaId, servicioIds);

            if (cancha == null)
                throw new NotFoundException($"No se encontró una cancha con id '{canchaId}'.");

            return cancha.ToCanchaResponse();
        }

        public async Task<CanchaResponse> AsociarVestuarioAsync(Guid canchaId, Guid vestuarioId)
        {
            var cancha = await _canchaRepository.AsociarVestuarioAsync(canchaId, vestuarioId);

            if (cancha == null)
                throw new NotFoundException($"No se encontró la cancha o el vestuario especificado.");

            return cancha.ToCanchaResponse();
        }
    }
}
