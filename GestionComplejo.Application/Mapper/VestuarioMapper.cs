using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Domain.Entities;

namespace GestionComplejo.Application.Mapper
{
    public static class VestuarioMapper
    {
        public static VestuarioResponse ToVestuarioResponse(this Vestuario vestuario)
        {
            return new VestuarioResponse
            {
                Id = vestuario.Id,
                NumeroVestuarios = vestuario.NumeroVestuarios,
                TieneDuchas = vestuario.TieneDuchas,
                Capacidad = vestuario.Capacidad,
                CanchaId = vestuario.CanchaId
            };
        }

        public static Vestuario ToVestuario(this VestuarioRequest request)
        {
            return new Vestuario
            {
                Id = Guid.NewGuid(),
                NumeroVestuarios = request.NumeroVestuarios,
                TieneDuchas = request.TieneDuchas,
                Capacidad = request.Capacidad,
                CanchaId = request.CanchaId,
                IsDeleted = false
            };
        }
    }
}
