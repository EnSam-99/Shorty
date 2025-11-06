  using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Request;
using Shorty.Domain.Services;
namespace Shorty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortyController(IShortyUrlService shortyUrlService, IShortyStatisticsService statisticsService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateShorty([FromBody] ShortyCreateRequestModel model)
        {
            var shorty = await shortyUrlService.CreateShortyAsync(model);
            return Ok(shorty);
        }

        [HttpGet("/{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            var originalUrl = await shortyUrlService.GetOriginalUrlAsync(shortCode);

            if (originalUrl == null)
                return NotFound("Short URL not found");

            await statisticsService.AddVisitAsync(shortCode);

            return Redirect(originalUrl);
        }
    }
}