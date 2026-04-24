using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GestionComplejo.Presentation.Controllers
{
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
                return NotFound();

            return Ok(servicios);
        }

        [HttpGet("{id}")]
        public ActionResult<ServicioResponse> GetById([FromRoute] Guid id)
        {
            var servicio = _servicioService.GetById(id);

            if (servicio == null)
                return NotFound();

            return Ok(servicio);
        }

        [HttpPost]
        public ActionResult<ServicioResponse> Create([FromBody] ServicioRequest request)
        {
            var created = _servicioService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] ServicioRequest request, [FromRoute] Guid id)
        {
            var updated = _servicioService.Update(request, id);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            _servicioService.Delete(id);
            return NoContent();
        }
    }
}
