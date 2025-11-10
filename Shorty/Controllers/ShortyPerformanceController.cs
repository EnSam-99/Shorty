using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Controllers
{

    [ApiController]
    [Route("api/shorties")]
    public class ShortyPerformanceController(IShortyPerformanceService performanceService) : ControllerBase
    {
        private readonly IShortyPerformanceService _performanceService;

        [HttpPost("recalculate-scores")]
        public async Task<IActionResult> RecalculateScores()
        {
            await _performanceService.RecalculateScoresAsync();
            return Ok(new { message = "Performance scores recalculated successfully." });
        }

        [HttpGet("top-performance")]
        public async Task<IActionResult> GetTopPerformance([FromQuery] int limit = 10)
        {
            var result = await _performanceService.GetTopPerformingShortiesAsync(limit);
            return Ok(result);
        }
    }
}
