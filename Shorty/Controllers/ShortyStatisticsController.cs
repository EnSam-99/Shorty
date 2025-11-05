using Microsoft.AspNetCore.Mvc;
using Shorty.Dal;
using Shorty.Domain.Services;

namespace Shorty.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ShortyStatisticsController : ControllerBase
    {
        private readonly ShortyStatisticsService _statisticsService;

        public ShortyStatisticsController(ShortyStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var stats = await _statisticsService.GetStatisticsAsync();
            return Ok(stats);
        }
    }
}
