using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Request;
namespace Shorty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortyController(IShortyUrlService shortyUrlService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateShorty([FromBody] ShortyCreateRequestModel model)
        {
            var shorty = await shortyUrlService.CreateShortyAsync(model);

            return Ok(shorty);
        }
    }
}