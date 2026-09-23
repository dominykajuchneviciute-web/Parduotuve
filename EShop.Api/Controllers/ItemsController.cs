using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok("Sveiki! Backend serveris veikia sėkmingai.");
        }
    }
}
