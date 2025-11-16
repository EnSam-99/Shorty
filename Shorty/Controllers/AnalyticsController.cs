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
                Clicks = s.Clicks,
                LastClickedAt = s.LastClickAt
            })
            .ToList() ?? new List<TopShortLinkDto>();

        return Ok(links);
    }

    [HttpPost("recalculate-score")]
    public async Task<IActionResult> UpdateAllScore()
    {
        await analyticsService.RecalculateScoresAsinc();

        return Ok();
    }
}