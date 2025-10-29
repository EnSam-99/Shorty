using Shorty.Dal;
using Shorty.Dal.Entities;
using Shorty.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
namespace Shorty.Services
{
    public class ShortyService
    {
        private readonly AppDbContext _db;

        public ShortyService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<string> CreateShortyAsync(CreateRequestModel request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                user = new User { Email = request.Email };
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
            }

            string shortCode = "";
            do
            {
                shortCode = Guid.NewGuid().ToString().Substring(0, 6);
            } while (await _db.Shorties.AnyAsync(s => s.ShortUrl == shortCode));

            var shorty = new Shorty.Dal.Entities.Shorty()
            {
                Url = request.Url,
                ShortUrl = shortCode,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id
            };
            await _db.Shorties.AddAsync(shorty);
            await _db.SaveChangesAsync();

            return shortCode;
        }
    }
} 