using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services.IServices;

[ApiController]
[Route("api/[controller]")]
public class ShortCodeHistoryController : ControllerBase
{
    private readonly IShortCodeHistoryService _svc;
    public ShortCodeHistoryController(IShortCodeHistoryService svc) => _svc = svc;

    [HttpGet("all-history")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await _svc.GetAllShortsHistoryAsync());

    [HttpPost] 
    public async Task<IActionResult> Create([FromBody] CreateHistoryRequest req)
    {
        await _svc.CreateHistoryAsync(req.ShortyId, req.OldShort, req.NewShort);
        return Ok();
    }

    public sealed class CreateHistoryRequest
    {
        public Guid ShortyId { get; set; }
        public string? OldShort { get; set; }
        public string NewShort { get; set; } = null!;
    }
}
