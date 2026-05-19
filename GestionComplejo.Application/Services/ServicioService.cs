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

        public List<ServicioResponse> GetAll()
        {
            return _servicioRepository
                .GetAll()
                .Select(x => x.ToServicioResponse())
                .ToList();
        }

        public ServicioResponse GetById(Guid id)
        {
            var servicio = _servicioRepository.GetById(id);

            if (servicio == null)
                throw new NotFoundException($"No se encontró un servicio con id '{id}'.");

            return servicio.ToServicioResponse();
        }

        public ServicioResponse Create(ServicioRequest request)
        {
            var newServicio = request.ToServicio();
            _servicioRepository.Add(newServicio);
            return newServicio.ToServicioResponse();
        }

        public void Update(ServicioRequest request, Guid id)
        {
            var servicio = _servicioRepository.GetById(id);

            if (servicio == null)
                throw new NotFoundException($"No se encontró un servicio con id '{id}'.");

            servicio.Nombre = request.Nombre;
            servicio.Descripcion = request.Descripcion;
            servicio.CostoAdicional = request.CostoAdicional;

            _servicioRepository.Update(servicio);
        }

        public void Delete(Guid id)
        {
            var servicio = _servicioRepository.GetById(id);

            if (servicio == null)
                throw new NotFoundException($"No se encontró un servicio con id '{id}'.");

            _servicioRepository.Delete(id);
        }
    }
}
