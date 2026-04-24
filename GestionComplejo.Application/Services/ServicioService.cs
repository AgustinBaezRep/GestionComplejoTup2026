using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Abstractions.Infrastructure;
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

        public ServicioResponse? GetById(Guid id)
        {
            return _servicioRepository.GetById(id)?.ToServicioResponse();
        }

        public ServicioResponse Create(ServicioRequest request)
        {
            var newServicio = request.ToServicio();
            _servicioRepository.Add(newServicio);
            return newServicio.ToServicioResponse();
        }

        public bool Update(ServicioRequest request, Guid id)
        {
            var servicio = _servicioRepository.GetById(id);

            if (servicio == null)
                return false;

            servicio.Nombre = request.Nombre;
            servicio.Descripcion = request.Descripcion;
            servicio.CostoAdicional = request.CostoAdicional;

            _servicioRepository.Update(servicio);
            return true;
        }

        public bool Delete(Guid id)
        {
            _servicioRepository.Delete(id);
            return true;
        }
    }
}
