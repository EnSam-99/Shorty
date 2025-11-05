using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Dal.Entities;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Request;
using Shorty.Domain.Models.Response;

namespace Shorty.Domain.Services;

public class ShortyUrlService(AppDbContext dbContext) : IShortyUrlService
{
    public async Task<ShortyCreateDto> CreateShortyAsync(ShortyCreateRequestModel model)
        {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            user = new User { Email = model.Email };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        var existingShorty = await dbContext.Shorties.FirstOrDefaultAsync(s => s.Url == model.Url && s.UserId == user.Id);

        if (existingShorty != null)
        {
            return new ShortyCreateDto()
            {
                Id = existingShorty.Id,
                ShortyUrl = existingShorty.ShortyUrl
            };
        }

        var shortCode = Guid.NewGuid().ToString().Substring(0, 6);

        var newShorty = new ShortLink
        {
            Url = model.Url,
            ShortyUrl = shortCode,
            CreatedDate = DateTime.UtcNow,
            UserId = user.Id,
          //  Clicks = 0
        };

        await dbContext.Shorties.AddAsync(newShorty);
        await dbContext.SaveChangesAsync();

        return new ShortyCreateDto()
        {
            Id = newShorty.Id,
            ShortyUrl = newShorty.ShortyUrl
        };
    }
    public async Task<string?> GetOriginalUrlAsync(string shortCode)
    {
        var shortLink = await dbContext.Shorties.FirstOrDefaultAsync(s => s.ShortyUrl == shortCode);
        if (shortLink == null)
            return null;

        dbContext.Visits.Add(new Visit
        {
            ShortLinkId = shortLink.Id,
            VisitDate = DateTime.UtcNow,
        });

        await dbContext.SaveChangesAsync();

        return shortLink?.Url;
    }

    
}
