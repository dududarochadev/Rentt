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
        private readonly DeliverymanService _deliverymanService;
        private readonly UserManager<User> _userManager;

        public DeliverymanController(
            DeliverymanService deliverymanService,
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
                return Unauthorized("Usuário não autenticado.");
            }

            if (file is null)
            {
                return NotFound();
            }

            var deliveryman = _deliverymanService.GetByUserId(user.Id);

            if (deliveryman == null)
            {
                return Unauthorized("Usuário não é entregador.");
            }

            try
            {
                var imageUrl = _deliverymanService.UpdateDriverLicenseImage(deliveryman, file);
                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar usuário: {ex.Message}");
            }
        }
    }
}
