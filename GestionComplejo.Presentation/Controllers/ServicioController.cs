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
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;

        public ServicioController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServicioResponse>>> GetAllAsync()
        {
            var servicios = await _servicioService.GetAllAsync();

            if (!servicios.Any())
                return NotFound("No hay servicios registrados.");

            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _servicioService.GetByIdAsync(id));
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public async Task<ActionResult<ServicioResponse>> CreateAsync([FromBody] ServicioRequest request)
        {
            var created = await _servicioService.CreateAsync(request);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] ServicioRequest request, [FromRoute] Guid id)
        {
            await _servicioService.UpdateAsync(request, id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _servicioService.DeleteAsync(id);
            return NoContent();
        }
    }
}
