using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shorty.Dal.DbModel.Repositories;
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
    private readonly IUrlService _service;

    public ShortyController(IUrlService service)
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
            ShortUrl = entity.ShortUrl,
            UserId = entity.UserId
        });

    }

    //[HttpGet]
    //public async Task<IActionResult> GetAllAsync()
    //{
    //    var shorties = await _service.GetAllShortyAsync();
    //    var result = shorties.Select( s => new ShortyDto()
    //    {
    //        UserId = s.UserId,
    //        ShortUrl = s.ShortUrl,
    //        Url = s.Url

    //    });

    //    return Ok(result);
    //}


}
