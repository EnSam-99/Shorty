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
        public async Task<IActionResult> GetTopShortLink(string email)
        {
            var links = await _analyticsService.GetTopShortLinkAsync(email);

            if (links == null || !links.Any())
            {
                return NotFound("No links found for the specified email.");
            }

            return Ok(links);
        }

		[HttpGet("top-performance")]
		public async Task<IActionResult> GetTopLinks()
		{
			var links = await _analyticsService.GetTopShortLinksAsync();

			if (links == null || !links.Any())
			{
				return NotFound("No links found for the specified email.");
			}

			return Ok(links);
		}

		[HttpPost("recalculate-scores")]
		public async Task<IActionResult> RecalculateScoresAsync()
		{
		    await _analyticsService.RecalculateScoresAsync();

            return Ok("Scores recalculated successfully.");
		}
	}
}
