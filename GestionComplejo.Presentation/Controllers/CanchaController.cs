using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Presentation.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionComplejo.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CanchaController : ControllerBase
    {
        private readonly ICanchaService _canchaService;

        public CanchaController(ICanchaService canchaService)
        {
            _canchaService = canchaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CanchaResponse>>> GetAllAsync()
        {
            var canchas = await _canchaService.GetAllAsync();

            if (!canchas.Any())
                return NotFound("No hay canchas registradas.");

            return Ok(canchas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CanchaResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _canchaService.GetByIdAsync(id));
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public async Task<ActionResult<CanchaResponse>> CreateAsync([FromBody] CanchaRequest cancha)
        {
            var createdCancha = await _canchaService.CreateAsync(cancha);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdCancha.Id }, createdCancha);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _canchaService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] CanchaRequest cancha, [FromRoute] Guid id)
        {
            await _canchaService.UpdateAsync(cancha, id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost("{id}/servicios")]
        public async Task<ActionResult<CanchaResponse>> AsociarServiciosAsync([FromRoute] Guid id, [FromBody] AsociarServiciosRequest request)
        {
            return Ok(await _canchaService.AsociarServiciosAsync(id, request.ServicioIds));
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost("{id}/vestuario/{vestuarioId}")]
        public async Task<ActionResult<CanchaResponse>> AsociarVestuarioAsync([FromRoute] Guid id, [FromRoute] Guid vestuarioId)
        {
            return Ok(await _canchaService.AsociarVestuarioAsync(id, vestuarioId));
        }
    }
}
