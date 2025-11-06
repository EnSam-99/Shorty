using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shorty.Dal.Db.Repositories;
using Shorty.Dal.Entities;
using Shorty.Dal.Models;
using Shorty.Domain.Models;
using Shorty.Models;
using Shorty.Services;
using Shorty.Services.IServices;

namespace Shorty.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShortyController: ControllerBase
{
    private readonly IUrlService<ShortyEntity> _service;

    public ShortyController(IUrlService<ShortyEntity> service)
    {
        _service = service;
    }

    [HttpPost("add-shorty")]
    public async Task<IActionResult> AddShorty([FromBody] CreateShortyRequestDto req)
    {

        var entity = await _service.CreateShortAsync(req.Url, req.UserId);
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

        if (string.IsNullOrWhiteSpace(shorty.NewShort))
            return BadRequest("NewShort is required.");
        await _service.UpdateShortCodAsync(shorty.ShortyId, shorty.NewShort);
        
        return Ok();
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllShorties()
    {
        var list = await _service.GetAllShortCodesAsync();
        var dto = list.Select(x => new ShortyDto
        {
            Id = x.Id,
            Url = x.Url,
            ShortUrl = x.ShortCode,
            UserId = x.UserId
        });
        return Ok(dto);
    }
  

}
