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
        public ActionResult<List<ServicioResponse>> GetAll()
        {
            var servicios = _servicioService.GetAll();

            if (!servicios.Any())
                return NotFound("No hay servicios registrados.");

            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public ActionResult<ServicioResponse> GetById([FromRoute] Guid id)
        {
            return Ok(_servicioService.GetById(id));
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public ActionResult<ServicioResponse> Create([FromBody] ServicioRequest request)
        {
            var created = _servicioService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public ActionResult Update([FromBody] ServicioRequest request, [FromRoute] Guid id)
        {
            _servicioService.Update(request, id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            _servicioService.Delete(id);
            return NoContent();
        }
    }
}
