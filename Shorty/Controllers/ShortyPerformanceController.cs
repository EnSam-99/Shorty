using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services;

namespace Shorty.Controllers;

[ApiController]
[Route("shorties")]
public class ShortyPerformanceController(PerformanceService _performanceService) : ControllerBase
{
    [HttpPost("recalculate-scores")]
    public async Task<IActionResult> RecalculateScores()
    {
        await _performanceService.RecalculateScoresAsync();
        return Ok(new { message = "Scores recalculated successfully." });
    }

    [HttpGet("top-performance")]
    public async Task<IActionResult> GetTopPerformance([FromQuery] int topN = 10)
    {
        var topShorties = await _performanceService.GetTopShortiesAsync(topN);
        return Ok(topShorties);
    }

}

