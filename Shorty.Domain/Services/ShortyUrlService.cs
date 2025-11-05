using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Request;
using Shorty.Domain.Models.Response;

namespace Shorty.Domain.Services;

public class ShortyUrlService : IShortyUrlService
{
    private readonly AppDbContext _context;

    public ShortyUrlService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ShortyCreateDto> CreateShortyAsync(ShortyCreateRequestModel model)
    {
        var shortCode = Guid.NewGuid().ToString().Substring(0, 6);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            user = new User { Email = model.Email };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        var newShorty = new ShortLink
        {
            Url = model.Url,
            ShortyUrl = shortCode,
            CreatedDate = DateTime.UtcNow,
            UserId = user.Id
        };

        await _context.Shorties.AddAsync(newShorty);
        await _context.SaveChangesAsync();


        return new ShortyCreateDto
        {
            Id = newShorty.Id,
            ShortyUrl = newShorty.ShortyUrl
        };
    }
    public async Task<string?> GetOriginalUrlAsync(string shortCode)
    {
        var shortLink = await _context.Shorties
                 .FirstOrDefaultAsync(s => s.ShortyUrl == shortCode);

        if (shortLink == null)
            return null;

        var visit = new Visit
        {
            ShortLinkId = shortLink.Id,
            CreatedDate = DateTime.UtcNow
        };

        _context.Visits.Add(visit);
        await _context.SaveChangesAsync();

        return shortLink.Url;
    }
}

