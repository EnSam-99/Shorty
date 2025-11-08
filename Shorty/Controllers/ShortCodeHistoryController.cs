using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services.Abstractions;

[ApiController]
[Route("api/[controller]")]
public class ShortCodeHistoryController(IShortCodeHistoryService _service) : ControllerBase
{
    [HttpGet("all-history")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await _service.GetAllShortsHistoryAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHistoryRequest req)
    {
        await _service.CreateHistoryAsync(req.ShortyId, req.OldShort, req.NewShort);
        return Ok();
    }

    public sealed class CreateHistoryRequest
    {
        public int ShortyId { get; set; }
        public string? OldShort { get; set; }
        public string NewShort { get; set; } = null!;
    }
}
