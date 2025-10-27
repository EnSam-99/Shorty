using Microsoft.AspNetCore.Mvc;
using Shorty.Dal;
using Shorty.Dal.Models;
using Shorty.Models;

namespace Shorty.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShortyController: ControllerBase
{
    readonly ShortyUrlRepository _shortyUrlRepository;

    public ShortyController(ShortyUrlRepository shortyUrlRepository)
    {
        _shortyUrlRepository = shortyUrlRepository;
    }

    [HttpPost("add-shorty")]
    public async Task<IActionResult> AddShorty([FromBody] ShortyDto shorty)
    {
       if(string.IsNullOrEmpty(shorty.Url) || string.IsNullOrEmpty(shorty.ShortUrl))
        {
            return BadRequest("Empty fields");
        }

        var shortyurl = new ShortyModel
        {
            Url = shorty.Url,
            ShortUrl = shorty.ShortUrl,
            UserId = shorty.UserId
        };

        await _shortyUrlRepository.AddShortyAsync(shortyurl);
        return Ok();

    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var shorties = await _shortyUrlRepository.GetAllShortyAsync();
        var result = shorties.Select( s => new ShortyDto()
        {
            UserId = s.UserId,
            ShortUrl = s.ShortUrl,
            Url = s.Url

        });

        return Ok(result);
    }


}
