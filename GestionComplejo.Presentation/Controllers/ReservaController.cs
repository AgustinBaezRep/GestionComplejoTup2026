using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComplejo.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost]
        public ActionResult<ReservaResponse> Create([FromBody] ReservaRequest request)
        {
            var reserva = _reservaService.Create(request);

            if (reserva == null)
                return Conflict("La cancha ya tiene una reserva en ese horario.");

            return StatusCode(StatusCodes.Status201Created, reserva);
        }
    }
}
