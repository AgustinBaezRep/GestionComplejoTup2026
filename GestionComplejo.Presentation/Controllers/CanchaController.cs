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
        public ActionResult<List<CanchaResponse>> GetAll()
        {
            var canchas = _canchaService.GetAll();

            if (!canchas.Any())
                return NotFound("No hay canchas registradas.");

            return Ok(canchas);
        }

        [HttpGet("{id}")]
        public ActionResult<CanchaResponse> GetById([FromRoute] Guid id)
        {
            return Ok(_canchaService.GetById(id));
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public ActionResult<CanchaResponse> Create([FromBody] CanchaRequest cancha)
        {
            var createdCancha = _canchaService.Create(cancha);
            return CreatedAtAction(nameof(GetById), new { id = createdCancha.Id }, createdCancha);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            _canchaService.Delete(id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public ActionResult Update([FromBody] CanchaRequest cancha, [FromRoute] Guid id)
        {
            _canchaService.Update(cancha, id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost("{id}/servicios")]
        public ActionResult<CanchaResponse> AsociarServicios([FromRoute] Guid id, [FromBody] AsociarServiciosRequest request)
        {
            return Ok(_canchaService.AsociarServicios(id, request.ServicioIds));
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost("{id}/vestuario/{vestuarioId}")]
        public ActionResult<CanchaResponse> AsociarVestuario([FromRoute] Guid id, [FromRoute] Guid vestuarioId)
        {
            return Ok(_canchaService.AsociarVestuario(id, vestuarioId));
        }
    }
}
