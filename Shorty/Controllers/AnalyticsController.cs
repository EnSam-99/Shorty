using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    [HttpGet("top/{email}")]
    public async Task<IActionResult> GetTopLinks(string email, [FromQuery] int limit = 10)
    {
        var links = await analyticsService.GetTopShortLinksByEmailAsync(email, limit);

        if (links == null || links.Count == 0)
        {
            return NotFound("No links found for the specified email.");
        }

        return Ok(links);
    }

	[HttpPost("recalculate-scores")]
	public async Task<IActionResult> RecalculateScores([FromQuery]int defaultBatchSize = 100)
	{
		if (defaultBatchSize < 1)
			return BadRequest("Batch size must be >= 1");

		await analyticsService.RecalculateScoresAsync(defaultBatchSize);
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