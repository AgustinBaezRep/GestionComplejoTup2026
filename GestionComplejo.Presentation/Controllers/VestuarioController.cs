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
        public ActionResult<List<VestuarioResponse>> GetAll()
        {
            var vestuarios = _vestuarioService.GetAll();

            if (!vestuarios.Any())
                return NotFound();

            return Ok(vestuarios);
        }

        [HttpGet("{id}")]
        public ActionResult<VestuarioResponse> GetById([FromRoute] Guid id)
        {
            var vestuario = _vestuarioService.GetById(id);

            if (vestuario == null)
                return NotFound();

            return Ok(vestuario);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public ActionResult<VestuarioResponse> Create([FromBody] VestuarioRequest request)
        {
            var created = _vestuarioService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public ActionResult Update([FromBody] VestuarioRequest request, [FromRoute] Guid id)
        {
            var updated = _vestuarioService.Update(request, id);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            _vestuarioService.Delete(id);
            return NoContent();
        }
    }
}
