using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.DTOs;
using Shorty.Services;

namespace Shorty.Controllers;
[ApiController]
[Route("api/shorty")]
public class ShortyController : ControllerBase
{
    private readonly ShortyService _shortyService;

    public ShortyController(ShortyService shortyService)
    {
        _shortyService = shortyService;
    }
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateRequestModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        string shortCode = await _shortyService.CreateShortyAsync(request);
        return Created($"/api/shorty/{shortCode}", new { ShortUrl = shortCode });
    }
}