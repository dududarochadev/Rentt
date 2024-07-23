using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rentt.Entities;
using Rentt.Models;
using Rentt.Services;

namespace Rentt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDeliverymanService _deliverymanService;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IDeliverymanService deliverymanService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _deliverymanService = deliverymanService;
        }

        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateAdminModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");

                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("registerDeliveryman")]
        public async Task<IActionResult> RegisterDeliveryman([FromBody] CreateDeliverymanModel model)
        {
            _deliverymanService.ValidateDeliveryman(model);

            var user = new User
            {
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Deliveryman");

                _deliverymanService.Create(model, user.Id);

                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpGet]
        public async Task<User?> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}