using Microsoft.AspNetCore.Mvc;
using Shorty.Dal.Models;
using Shorty.Services.IServices;

namespace Shorty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedirectController: ControllerBase
    {
        private readonly IUrlService<ShortyEntity> _service;

        public RedirectController(IUrlService<ShortyEntity> service)=> _service = service;        

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
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
