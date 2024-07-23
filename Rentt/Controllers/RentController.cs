using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rentt.Entities;
using Rentt.Models;
using Rentt.Services;

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentModel newRent)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return Unauthorized("Usuário não autenticado.");
                }

                var createdRent = _rentService.Create(newRent, user);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
