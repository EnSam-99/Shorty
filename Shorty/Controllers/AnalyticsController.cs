using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    [HttpPost("recalculate-scores")]
    public async Task<IActionResult> RecalculateScores()
    {
        try
        {
            await analyticsService.RecalculateAllScoresAsync();
            return Ok(new { message = "Scores recalculated successfully" });
        }
        catch (Exception ex)
        {
            // ← ԱՅՍՏԵՂ ՏԵՍՆԵԼՈՒ ԵՆՔ EXACT ERROR-Ը
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
    [HttpGet("top-performance")]
    public async Task<IActionResult> GetTopPerformers([FromQuery] int limit = 10)
    {
        var result = await analyticsService.GetTopPerformingShortiesAsync(limit);
        return Ok(result);
    }
}