using Microsoft.AspNetCore.Mvc;
using Rentt.Entities;
using Rentt.Services;

namespace Rentt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcycleController : ControllerBase
    {
        private readonly MotorcycleService _motorcycleService;

        public MotorcycleController(MotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Motorcycle>> GetAll()
        {
            var motorcycles = _motorcycleService.GetAll();
            return Ok(motorcycles);
        }

        [HttpGet("{id:length(24)}", Name = "GetMotorcycle")]
        public ActionResult<Motorcycle> GetById(string id)
        {
            var motorcycle = _motorcycleService.GetById(id);
            if (motorcycle == null)
            {
                return NotFound();
            }
            return Ok(motorcycle);
        }

        [HttpPost]
        public ActionResult<Motorcycle> Create([FromBody] Motorcycle newMotorcycle)
        {
            try
            {
                var createdMotorcycle = _motorcycleService.Create(newMotorcycle);
                return CreatedAtRoute("GetMotorcycle", new { id = createdMotorcycle.Id }, createdMotorcycle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult Update(string id, [FromBody] string newLicensePlate)
        {
            try
            {
                _motorcycleService.Update(id, newLicensePlate);
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
