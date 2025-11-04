using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Request;

namespace Shorty.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ShortyController : ControllerBase
	{
		private readonly IShortyUrlService _service;

		public ShortyController(IShortyUrlService service)
		{
			_service = service;
		}

		
		[HttpPost]
		public async Task<IActionResult> CreateShorty([FromBody] ShortyCreateRequestModel model)
		{
			var shorty = await _service.CreateShortyAsync(model);
			return Ok(shorty);
		}

		
		[HttpGet("/r/{code}")]
		public async Task<IActionResult> RedirectByCode(string code)
		{
			var url = await _service.ResolveAndTrackAsync(code);
			if (url is null)
				return NotFound();

			return Redirect(url);
		}

		
		[HttpGet("stats/{code}")]
		public async Task<IActionResult> GetStatsByCode(string code)
		{
			var stats = await _service.GetStatsByCodeAsync(code);
			if (stats is null)
				return NotFound();

			return Ok(stats);
		}

		
		[HttpGet("top")]
		public async Task<IActionResult> GetTopShorties([FromQuery] int take = 10)
		{
			var result = await _service.GetTopShortiesAsync(take);
			return Ok(result);
		}
	}
}
