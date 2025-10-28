using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Data;
using Shorty.Dal.Entities;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class UrlController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public UrlController(ApplicationDbContext db)
    {
        _db = db;
    }

    // POST /api/url
    [HttpPost]
    public async Task<IActionResult> Shorten([FromBody] ShortenUrlRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OriginalUrl))
            return BadRequest("Original URL is required.");

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // generate short URL
        var shortUrl = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);

        var mapping = new UrlMapping
        {
            Id = Guid.NewGuid(),
            OriginalURL = request.OriginalUrl,
            ShortURL = shortUrl,
            UserId = userId
        };

        _db.UrlMappings.Add(mapping);
        await _db.SaveChangesAsync();

        return Ok(new { mapping.OriginalURL, mapping.ShortURL });
    }

    // GET /api/url
    [HttpGet]
    public async Task<IActionResult> GetUserUrls()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var urls = await _db.UrlMappings
            .Where(u => u.UserId == userId)
            .Select(u => new { u.OriginalURL, u.ShortURL })
            .ToListAsync();

        return Ok(urls);
    }

    // GET /api/url/{shortUrl}
    [AllowAnonymous]
    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> GetOriginal(string shortUrl)
    {
        var mapping = await _db.UrlMappings.FirstOrDefaultAsync(u => u.ShortURL == shortUrl);
        if (mapping == null) return NotFound();

        return Ok(new { mapping.OriginalURL });
    }
}
