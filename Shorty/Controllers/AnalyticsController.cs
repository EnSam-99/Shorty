using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Models.Response;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    [HttpGet("top-performance/limit=10")]
    public async Task<ActionResult<List<TopShortLinkDto>>> GetTopLinks()
    {
        var entities = await analyticsService.GetTopPerformingAsync();

        var links = entities?
            .Select(s => new TopShortLinkDto
            {
                ShortCode = s.ShortCode,
                Url = s.Url,
                Score = s.Score,
                Clicks = 0,//todo: set actual click count
                LastClickedAt = s.LastClickAt
            })
            .ToList() ?? [];

        return Ok(links);
    }

    [HttpPost("recalculate-score")]
    public async Task<IActionResult> UpdateAllScore()
    {
        await analyticsService.RecalculateScoresAsinc();

        return Ok();
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