using Microsoft.AspNetCore.Mvc;
using Shorty.Dal;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Services;

namespace Shorty.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ShortyStatisticsController : ControllerBase
    {
        private readonly IShortyStatisticsService _statisticsService;

        public ShortyStatisticsController(IShortyStatisticsService statisticsService)
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
