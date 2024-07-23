using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rentt.Entities;
using Rentt.Models;
using Rentt.Services;

namespace Rentt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleService _motorcycleService;

        public MotorcycleController(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Motorcycle>> Get(string? licensePlate)
        {
            var motorcycles = _motorcycleService.Get(licensePlate);
            return Ok(motorcycles);
        }

        [HttpGet("{id:length(24)}")]
        public ActionResult<Motorcycle> GetById(string id)
        {
            var motorcycle = _motorcycleService.GetById(id);

            if (motorcycle is null)
            {
                return NotFound();
            }

            return Ok(motorcycle);
        }

        [HttpPost]
        public ActionResult<Motorcycle> Create([FromBody] CreateMotorcycleModel newMotorcycle)
        {
            try
            {
                var createdMotorcycle = _motorcycleService.Create(newMotorcycle);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult UpdateLicensePlate(string id, [FromBody] string newLicensePlate)
        {
            try
            {
                _motorcycleService.UpdateLicensePlate(id, newLicensePlate);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult Delete(string id)
        {
            try
            {
                _motorcycleService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
