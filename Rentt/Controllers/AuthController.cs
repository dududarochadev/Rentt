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

        /// <summary>
        /// Cria um novo usuário do tipo admin
        /// </summary>
        /// <param name="model">Model de criação do usuário do tipo admin</param>
        /// <returns>Objeto criado.</returns>
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

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(user);
        }

        /// <summary>
        /// Cria um novo usuário do tipo entregador
        /// </summary>
        /// <param name="model">Model de criação do usuário do tipo entregador</param>
        /// <returns>Objeto criado.</returns>
        [HttpPost("registerDeliveryman")]
        public async Task<IActionResult> RegisterDeliveryman([FromBody] CreateDeliverymanModel model)
        {
            var resultValidate = _deliverymanService.ValidateDeliveryman(model);

            if (!resultValidate.Success)
            {
                return BadRequest(resultValidate);
            }

            var user = new User
            {
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Deliveryman");

            var resultCreateDeliveryman = _deliverymanService.Create(model, user.Id);

            if (!resultCreateDeliveryman.Success)
            {
                return BadRequest(resultCreateDeliveryman);
            }

            return Ok(resultCreateDeliveryman);

        }

        /// <summary>
        /// Realiza o login na aplicação
        /// </summary>
        /// <param name="model">Model de login</param>
        /// <returns>Ok.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            return Ok();
        }

        /// <summary>
        /// Obtém o usuário logado na aplicação.
        /// </summary>
        /// <returns>Objeto do usuário logado na aplicação</returns>
        [HttpGet]
        public async Task<User?> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}