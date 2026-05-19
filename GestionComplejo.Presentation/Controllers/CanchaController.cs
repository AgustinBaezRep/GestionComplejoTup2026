using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Exceptions;
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
            try
            {
                var canchas = _canchaService.GetAll();

                if (!canchas.Any())
                    return NotFound("No hay canchas registradas.");

                return Ok(canchas);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CanchaResponse> GetById([FromRoute] Guid id)
        {
            try
            {
                return Ok(_canchaService.GetById(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public ActionResult<CanchaResponse> Create([FromBody] CanchaRequest cancha)
        {
            try
            {
                var createdCancha = _canchaService.Create(cancha);
                return CreatedAtAction(nameof(GetById), new { id = createdCancha.Id }, createdCancha);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                _canchaService.Delete(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public ActionResult Update([FromBody] CanchaRequest cancha, [FromRoute] Guid id)
        {
            try
            {
                _canchaService.Update(cancha, id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost("{id}/servicios")]
        public ActionResult<CanchaResponse> AsociarServicios([FromRoute] Guid id, [FromBody] AsociarServiciosRequest request)
        {
            try
            {
                return Ok(_canchaService.AsociarServicios(id, request.ServicioIds));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost("{id}/vestuario/{vestuarioId}")]
        public ActionResult<CanchaResponse> AsociarVestuario([FromRoute] Guid id, [FromRoute] Guid vestuarioId)
        {
            try
            {
                return Ok(_canchaService.AsociarVestuario(id, vestuarioId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }
    }
}
