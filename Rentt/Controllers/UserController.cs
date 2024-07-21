using Microsoft.AspNetCore.Mvc;
using Rentt.Entities;
using Rentt.Services;

namespace Rentt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(string id)
        {
            try
            {
                var user = _userService.GetById(id);

                if (user is null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar usuário: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] User user)
        {
            try
            {
                var createdUser = _userService.Create(user);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar usuário: {ex.Message}");
            }
        }

        [HttpPut]
        public ActionResult<User> UpdateDriverLicenseImage(string id, [FromForm] IFormFile? file)
        {
            if (file is null)
            {
                return NotFound();
            }

            try
            {
                var imageUrl = _userService.UpdateDriverLicenseImage(id, file);
                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar usuário: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                _userService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao deletar usuário: {ex.Message}");
            }
        }
    }
}
