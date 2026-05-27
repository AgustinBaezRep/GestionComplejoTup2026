using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GestionComplejo.Presentation.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        //[Authorize(Policy = Policies.SoloCliente)]
        [HttpPost]
        public async Task<ActionResult<ReservaResponse>> CreateAsync([FromBody] ReservaRequest request)
        {
            var reserva = await _reservaService.CreateAsync(request);
            return StatusCode(StatusCodes.Status201Created, reserva);
        }
    }
}
