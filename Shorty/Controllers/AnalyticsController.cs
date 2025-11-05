using Microsoft.AspNetCore.Mvc;
using Shorty.Dal;
using Shorty.Domain.Abstraction;

namespace Shorty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("top/{email}")]
        public async Task<IActionResult> GetTopLinks(string email)
        {
            var links = await _analyticsService.GetTopShortLinksAsync(email);

            if (links == null || !links.Any())
            {
                return NotFound("No links found for the specified email.");
            }

            return Ok(links);
        }
        
    }
}
