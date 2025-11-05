using Microsoft.AspNetCore.Mvc;
using Shorty.Dal;
using Shorty.Domain.Abstraction;


namespace Shorty.Controllers
{
    [ApiController]
    [Route("")]
    public class RedirectController : ControllerBase
    {
        private readonly IShortyUrlService _shortyUrlService;

        public RedirectController(IShortyUrlService shortyUrlService)
        {
            _shortyUrlService = shortyUrlService;
        }  

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
        {
            var originalUrl = await _shortyUrlService.GetOriginalUrlAsync(shortCode);
            if (originalUrl != null)
            {
                return Redirect(originalUrl);
            }
            return NotFound("Short URL not found.");
        }

    } 
}
