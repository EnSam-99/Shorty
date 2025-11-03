using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Request;
using Shorty.Domain.Services;
namespace Shorty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortyController : ControllerBase
    {
		private readonly IShortyUrlService _shortyUrlService;
		public ShortyController(IShortyUrlService shortyService)
		{
			_shortyUrlService = shortyService;
		}

		[HttpPost()]
        public async Task<IActionResult> CreateShorty([FromBody] ShortyCreateRequestModel model)
        {
            var shorty = await _shortyUrlService.CreateShortyAsync(model);

            return Ok(shorty);
        }

		[HttpGet("by-user/{email}")]
		public async Task<IActionResult> GetByUserEmail([FromRoute] string email)
		{
			var result = await _shortyUrlService.GetShortiesByEmailAsync(email);
			if (result is null || result.Count == 0)
				return NotFound($"No short links found for user: {email}");

			return Ok(result);
		}

		[HttpGet("/r/{code}")]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public async Task<IActionResult> RedirectByCode(string code)
		{
			var url = await _shortyUrlService.ResolveAndTrackAsync(code);
			if (url is null) return NotFound();
			return Redirect(url); 
		}

		[HttpGet("stats/{code}")]
		public async Task<IActionResult> GetStats([FromRoute] string code)
		{
			var dto = await _shortyUrlService.GetStatsAsync(code);
			if (dto is null) return NotFound();

			return Ok(dto);
		}
		[HttpGet("top")]
		public async Task<IActionResult> GetTop([FromQuery] int take = 10)
		{
			var list = await _shortyUrlService.GetTopAsync(take);
			return Ok(list.Select(x => new { short_code = x.ShortCode, clicks = x.Clicks }));
		}
	}
}
