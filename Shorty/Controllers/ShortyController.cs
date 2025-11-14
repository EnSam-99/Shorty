using Microsoft.AspNetCore.Mvc;
using Shorty.Dal.Entities;
using Shorty.Domain.Models;
using Shorty.Domain.Services.Abstractions;
using Shorty.Models;

namespace Shorty.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShortyController : ControllerBase 
{
	private readonly IShortyExpiryService shortyExpiryService;
	private readonly IUrlService<ShortyEntity> service;

	public ShortyController(IUrlService<ShortyEntity> service, IShortyExpiryService shortyExpiryService)
	{
		this.service = service;
		this.shortyExpiryService = shortyExpiryService;
	}

	[HttpPost("add-shorty")]
	public async Task<IActionResult> AddShorty([FromBody] CreateShortyRequestDto req)
	{
		var entity = await service.CreateShortAsync(req.Url, req.UserId);
		return Ok(new ShortyDto
		{
			Url = entity.Url,
			ShortUrl = entity.ShortCode,
			UserId = entity.UserId
		});
	}

	[HttpPatch("update-short")]
	public async Task<IActionResult> UpdateShortCode([FromBody] UpdateShortRequestDto shorty)
	{
		if (shorty is null)
			return BadRequest("Body is required.");

		if (shorty.ShortyId == 0)
			return BadRequest("ShortyId is required.");

		await service.UpdateShortCodAsync(shorty.ShortyId, shorty.NewShort);

		return Ok();
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetAllShorties()
	{
		var list = await service.GetAllShortCodesAsync();

		var dto = list.Select(x => new ShortyDto
		{
			Id = x.Id,
			Url = x.Url,
			ShortUrl = x.ShortCode,
			UserId = x.UserId
		});

		return Ok(dto);
	}

	[HttpGet("expired")]
	public async Task<IActionResult> GetExpiredShorties()
	{
		var expired = await shortyExpiryService.GetExpiredShortiesAsync();
		
		if (!expired.Any())
			return NotFound("No expired shorties found.");

		var result = expired.Select(x => new ShortyDto
		{
			Id = x.Id,
			Url = x.Url,
			ShortUrl = x.ShortCode,
			UserId = x.UserId
		});

		return Ok(result);
	}
}
