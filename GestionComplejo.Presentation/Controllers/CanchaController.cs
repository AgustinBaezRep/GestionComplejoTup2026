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
        public ActionResult<CanchaResponse> GetAll()
        {
            var canchas = _canchaService.GetAll();

            if (!canchas.Any())
                return NotFound();

            return Ok(canchas);
        }

        [HttpGet("{id}")]
        public ActionResult<CanchaResponse> GetById([FromRoute] Guid id)
        {
            var cancha = _canchaService.GetById(id);

            if (cancha == null)
                return NotFound();

            return Ok(cancha);
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
            var createdCancha = _canchaService.Delete(id);

            if (!createdCancha)
                return NotFound();

            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public ActionResult Update([FromBody] CanchaRequest cancha, [FromRoute] Guid id)
        {
            var updatedCancha = _canchaService.Update(cancha, id);

            if (!updatedCancha)
                return NotFound();

            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost("{id}/servicios")]
        public ActionResult<CanchaResponse> AsociarServicios([FromRoute] Guid id, [FromBody] AsociarServiciosRequest request)
        {
            var cancha = _canchaService.AsociarServicios(id, request.ServicioIds);

            if (cancha == null)
                return NotFound();

            return Ok(cancha);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost("{id}/vestuario/{vestuarioId}")]
        public ActionResult<CanchaResponse> AsociarVestuario([FromRoute] Guid id, [FromRoute] Guid vestuarioId)
        {
            var cancha = _canchaService.AsociarVestuario(id, vestuarioId);

            if (cancha == null)
                return NotFound();

            return Ok(cancha);
        }
    }
}
