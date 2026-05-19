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
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _servicioService;

        public ServicioController(IServicioService servicioService)
        {
            _servicioService = servicioService;
        }

        [HttpGet]
        public ActionResult<List<ServicioResponse>> GetAll()
        {
            try
            {
                var servicios = _servicioService.GetAll();

                if (!servicios.Any())
                    return NotFound("No hay servicios registrados.");

                return Ok(servicios);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ServicioResponse> GetById([FromRoute] Guid id)
        {
            try
            {
                return Ok(_servicioService.GetById(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public ActionResult<ServicioResponse> Create([FromBody] ServicioRequest request)
        {
            try
            {
                var created = _servicioService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public ActionResult Update([FromBody] ServicioRequest request, [FromRoute] Guid id)
        {
            try
            {
                _servicioService.Update(request, id);
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
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                _servicioService.Delete(id);
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
        }
    }
}
