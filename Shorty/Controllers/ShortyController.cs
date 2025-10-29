using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
namespace Shorty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShortyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShorty([FromBody] ShortyCreateDto shorty)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string shortCode = Guid.NewGuid().ToString().Substring(0, 6);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == shorty.Email);
            if (user == null)
            {
                user = new User { Email = shorty.Email };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            var newShorty = new ShortLink
            {
                Url = shorty.Url,
                ShortyUrl = shortCode,
                CreatedDate = DateTime.UtcNow,
                UserId = user.Id
            };

            _context.Shorties.Add(newShorty);
            await _context.SaveChangesAsync();

            return Ok(newShorty);
        }
    }
}
