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
    public class VestuarioController : ControllerBase
    {
        private readonly IVestuarioService _vestuarioService;

        public VestuarioController(IVestuarioService vestuarioService)
        {
            _vestuarioService = vestuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<VestuarioResponse>>> GetAllAsync()
        {
            var vestuarios = await _vestuarioService.GetAllAsync();

            if (!vestuarios.Any())
                return NotFound("No hay vestuarios registrados.");

            return Ok(vestuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VestuarioResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _vestuarioService.GetByIdAsync(id));
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public async Task<ActionResult<VestuarioResponse>> CreateAsync([FromBody] VestuarioRequest request)
        {
            var created = await _vestuarioService.CreateAsync(request);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] VestuarioRequest request, [FromRoute] Guid id)
        {
            await _vestuarioService.UpdateAsync(request, id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _vestuarioService.DeleteAsync(id);
            return NoContent();
        }
    }
}
