using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Controllers
{

    [ApiController]
    [Route("api/shorties")]
    public class ShortyPerformanceController(IShortyPerformanceService performanceService) : ControllerBase
    {
        [HttpPost("recalculate-scores")]
        public async Task<IActionResult> RecalculateScores()
        {
            await performanceService.RecalculateScoresAsync();
            return Ok(new { message = "Performance scores recalculated successfully." });
        }

        [HttpGet("top-performance")]
        public async Task<IActionResult> GetTopPerformance([FromQuery] int limit = 10)
        {
            var result = await performanceService.GetTopPerformingShortiesAsync(limit);
            return Ok(result);
        }
    }
}
