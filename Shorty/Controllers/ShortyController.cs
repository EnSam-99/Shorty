using Microsoft.AspNetCore.Mvc;
using Shorty.Dal.DbModel.Repositories;
using Shorty.Dal.Models;
using Shorty.Models;
using Shorty.Services;

namespace Shorty.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShortyController: ControllerBase
{
    private readonly UrlService _service;

    public ShortyController(UrlService service)
    {
        _service = service;
    }

    [HttpPost("add-shorty")]
    public async Task<IActionResult> AddShorty([FromBody] ShortyDto shorty)
    {

        var created = await _service.CreateShortAsync(shorty.Url, shorty.UserId);
        return Ok();

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
