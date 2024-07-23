using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rentt.Entities;
using Rentt.Services;

namespace Rentt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "DELIVERYMAN")]
    public class DeliverymanController : ControllerBase
    {
        private readonly IDeliverymanService _deliverymanService;
        private readonly UserManager<User> _userManager;

        public DeliverymanController(
            IDeliverymanService deliverymanService,
            UserManager<User> userManager)
        {
            _deliverymanService = deliverymanService;
            _userManager = userManager;
        }

        [HttpPut("updateDriverLicenseImage")]
        public async Task<IActionResult> UpdateDriverLicenseImage([FromForm] IFormFile? file)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized("Usu�rio n�o autenticado.");
            }

            if (file is null)
            {
                return NotFound();
            }

            var deliveryman = _deliverymanService.GetByUserId(user.Id);

            if (deliveryman == null)
            {
                return Unauthorized("Usu�rio n�o � entregador.");
            }

            var result = _deliverymanService.UpdateDriverLicenseImage(deliveryman, file);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
