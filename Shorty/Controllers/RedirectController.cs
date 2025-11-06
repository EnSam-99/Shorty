using Microsoft.AspNetCore.Mvc;
using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Models;
using Shorty.Services.IServices;

namespace Shorty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedirectController : ControllerBase
    {
        private readonly IUrlService<ShortyEntity> _service;
        private readonly IShortyUrlRepository<ShortyEntity> _urlRepo;
        public RedirectController(IUrlService<ShortyEntity> service, IShortyUrlRepository<ShortyEntity> urlRepo)
        {
            _service = service;
            _urlRepo = urlRepo;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            if (!await _urlRepo.IsShortCodeValidateAsync(code))
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
}
