using Microsoft.AspNetCore.Mvc;
using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Models;
using Shorty.Services.IServices;

namespace Shorty.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RedirectController(IUrlService<ShortyEntity> _service, IShortyUrlRepository<ShortyEntity> _urlRepo) : ControllerBase
{
    [HttpGet("{code}")]
    public async Task<IActionResult> Get(string code)
    {
        if (!await _urlRepo.IsShortCodeValidAsync(code))
        {
            return BadRequest("ShortCode is not valid or expired.");
        }
        var target = await _service.GetOriginalUrlAsync(code);

        if (string.IsNullOrWhiteSpace(target))
            return NotFound();
        if (!Uri.TryCreate(target, UriKind.Absolute, out var uri))
        {
            var guess = "https://" + target.Trim();
            if (!Uri.TryCreate(guess, UriKind.Absolute, out uri))
                return BadRequest("Invalid target URL.");
        }

        return Redirect(uri.ToString());
    }
}
