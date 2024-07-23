using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rentt.Entities;
using Rentt.Models;
using Rentt.Services;
using System.Runtime.CompilerServices;

namespace Rentt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "DELIVERYMAN")]
    public class RentController : ControllerBase
    {
        private readonly IRentService _rentService;
        private readonly UserManager<User> _userManager;

        public RentController(
            IRentService rentService,
            UserManager<User> userManager)
        {
            _rentService = rentService;
            _userManager = userManager;
        }

        /// <summary>
        /// Obt�m uma loca��o pelo Id.
        /// </summary>
        /// <param name="id">Id da loca��o a ser buscada.</param>
        /// <returns>Objeto buscado.</returns>
        [HttpGet("{id:length(24)}")]
        public IActionResult GetById(string id)
        {
            var rent = _rentService.GetById(id);

            if (rent is null)
            {
                return NotFound();
            }

            return Ok(rent);
        }

        /// <summary>
        /// Cria uma nova loca��o para o usu�rio logado.
        /// </summary>
        /// <param name="newRent">Model de cria��o de loca��o.</param>
        /// <returns>Objeto criado.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentModel newRent)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized("Usu�rio n�o autenticado.");
            }

            var result = _rentService.Create(newRent, user);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Created(nameof(GetById), result);
        }

        /// <summary>
        /// Calcula o valor total da loca��o pelo id.
        /// </summary>
        /// <param name="id">Id da loca��o a ser calculada.</param>
        /// <returns>O resultado obtendo o valor total da loca��o.</returns>
        [HttpGet("calculateTotalRentalCost/{id:length(24)}")]
        public IActionResult CalculateTotalRentalCost(string id)
        {
            var result = _rentService.CalculateTotalRentalCost(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Created(nameof(GetById), result);
        }
    }
}
