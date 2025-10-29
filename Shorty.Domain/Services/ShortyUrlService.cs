using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Request;
using Shorty.Domain.Models.Response;

namespace Shorty.Domain.Services;

public class ShortyUrlService(AppDbContext dbContext) : IShortyUrlService
{
    public async Task<ShortyCreateDto> CreateShortyAsync(ShortyCreateRequestModel model)
    {
        var shortCode = Guid.NewGuid().ToString().Substring(0, 6);
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            user = new User { Email = model.Email };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        var newShorty = new ShortLink
        {
            Url = model.Url,
            ShortyUrl = shortCode,
            CreatedDate = DateTime.UtcNow,
            UserId = user.Id
        };

        await dbContext.Shorties.AddAsync(newShorty);
        await dbContext.SaveChangesAsync();

        return new ShortyCreateDto()
        {
            Id = newShorty.Id,
            ShortyUrl = newShorty.ShortyUrl
        };
    }
}
