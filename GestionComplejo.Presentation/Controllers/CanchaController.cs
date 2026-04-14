using GestionComplejo.Application.Abstractions;
using GestionComplejo.Application.Requests;
using GestionComplejo.Application.Responses;
using GestionComplejo.Application.Services;
using GestionComplejo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestionComplejo.Presentation.Controllers
{
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
            return Ok(_canchaService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CanchaResponse> GetById([FromRoute] Guid id)
        {
            var cancha = _canchaService.GetById(id);

            if (cancha == null)
                return NotFound();

            return Ok(cancha);
        }

        [HttpPost]
        public ActionResult<CanchaResponse> Create([FromBody] CanchaRequest cancha)
        {
            var createdCancha = _canchaService.Create(cancha);

            return CreatedAtAction(nameof(GetById), new { id = createdCancha.Id }, createdCancha);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            var createdCancha = _canchaService.Delete(id);

            if (!createdCancha)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] CanchaRequest cancha, [FromRoute] Guid id)
        {
            var updatedCancha = _canchaService.Update(cancha, id);

            if (!updatedCancha)
                return NotFound();

            return NoContent();
        }
    }
}
