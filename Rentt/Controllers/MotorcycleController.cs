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

        /// <summary>
        /// Obt�m todas as motos cadastradas podendo filtrar pela placa.
        /// </summary>
        /// <param name="licensePlate">Filtro por placa da moto.</param>
        /// <returns>Lista de motos cadastradas.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Motorcycle>> Get(string? licensePlate)
        {
            var motorcycles = _motorcycleService.Get(licensePlate);
            return Ok(motorcycles);
        }

        /// <summary>
        /// Obt�m uma moto �nica pelo Id.
        /// </summary>
        /// <param name="id">Id da moto a ser buscada.</param>
        /// <returns>Objeto da moto obtida.</returns>
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

        /// <summary>
        /// Cria um objeto de moto
        /// </summary>
        /// <param name="newMotorcycle">Model de cria��o da moto</param>
        /// <returns>O objeto criado.</returns>
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

        /// <summary>
        /// Atualiza a placa de uma moto pelo id.
        /// </summary>
        /// <param name="id">Id da moto a ser atualizada.</param>
        /// <param name="newLicensePlate">Placa nova para atualiza��o.</param>
        /// <returns>Objeto atualizado.</returns>
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

        /// <summary>
        /// Deleta uma moto desde que ela n�o tenha registro de loca��es.
        /// </summary>
        /// <param name="id">Id da moto a ser deletada.</param>
        /// <returns>Resultado da oprea��o.</returns>
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
