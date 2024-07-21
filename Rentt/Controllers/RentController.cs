using Microsoft.AspNetCore.Mvc;
using Rentt.Entities;
using Rentt.Services;

namespace Rentt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentController : ControllerBase
    {
        private readonly RentService _rentService;

        public RentController(RentService rentService)
        {
            _rentService = rentService;
        }

        [HttpPost]
        public ActionResult<Rent> Create([FromBody] CreateRent newRent)
        {
            try
            {
                var createdRent = _rentService.Create(newRent);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
