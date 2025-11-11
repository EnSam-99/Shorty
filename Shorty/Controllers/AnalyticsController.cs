using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    [HttpGet("top/{email}")]
    public async Task<IActionResult> GetTopLinks(string email)
    {
        var links = await analyticsService.GetTopShortLinksAsync(email);

        if (links == null || links.Count == 0)
        {
            return NotFound("No links found for the specified email.");
        }

        return Ok(links);
    }

	[HttpPost("recalculate-scores")]
	public async Task<IActionResult> RecalculateScores()
	{
		await analyticsService.RecalculateScoresAsync();
		return Ok("Scores recalculated successfully.");
	}

	[HttpGet("top-performance")]
	public async Task<IActionResult> GetTopPerformance([FromQuery] int limit = 10)
	{
		var topShorties = await analyticsService.GetTopShortLinksAsync(limit);

		if (topShorties == null || !topShorties.Any())
			return NotFound(new { message = "No shorties found." });

		return Ok(topShorties);
	}

}