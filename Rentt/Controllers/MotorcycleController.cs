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
        public IActionResult Create([FromBody] CreateMotorcycleModel newMotorcycle)
        {
            var result = _motorcycleService.Create(newMotorcycle);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Created(nameof(GetById), result);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult UpdateLicensePlate(string id, [FromBody] string newLicensePlate)
        {
            var result = _motorcycleService.UpdateLicensePlate(id, newLicensePlate);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var result = _motorcycleService.Delete(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
