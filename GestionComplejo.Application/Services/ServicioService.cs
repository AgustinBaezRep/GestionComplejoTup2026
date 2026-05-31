using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Application.Exceptions;
using GestionComplejo.Application.Mapper;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;

namespace GestionComplejo.Application.Services
{
    public class ServicioService : IServicioService
    {
        private readonly IServicioRepository _servicioRepository;

        public ServicioService(IServicioRepository servicioRepository)
        {
            _servicioRepository = servicioRepository;
        }

        public async Task<List<ServicioResponse>> GetAllAsync()
        {
            return (await _servicioRepository.GetAllAsync())
                .Select(x => x.ToServicioResponse())
                .ToList();
        }

        public async Task<ServicioResponse> GetByIdAsync(Guid id)
        {
            var servicio = await _servicioRepository.GetByIdAsync(id);

            if (servicio == null)
                throw new NotFoundException($"No se encontró un servicio con id '{id}'.");

            return servicio.ToServicioResponse();
        }

        public async Task<ServicioResponse> CreateAsync(ServicioRequest request)
        {
            var newServicio = request.ToServicio();
            await _servicioRepository.AddAsync(newServicio);
            return newServicio.ToServicioResponse();
        }

        public async Task UpdateAsync(ServicioRequest request, Guid id)
        {
            var servicio = await _servicioRepository.GetByIdAsync(id);

            if (servicio == null)
                throw new NotFoundException($"No se encontró un servicio con id '{id}'.");

            servicio.Nombre = request.Nombre;
            servicio.Descripcion = request.Descripcion;
            servicio.CostoAdicional = request.CostoAdicional;

            await _servicioRepository.UpdateAsync(servicio);
        }

        public async Task DeleteAsync(Guid id)
        {
            var servicio = await _servicioRepository.GetByIdAsync(id);

            if (servicio == null)
                throw new NotFoundException($"No se encontró un servicio con id '{id}'.");

            await _servicioRepository.DeleteAsync(id);
        }
    }
}
