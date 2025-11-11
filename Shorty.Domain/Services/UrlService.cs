using Shorty.Dal.Entities;
using Shorty.Dal.Repositories.Abstractions;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Services;

public class UrlService(IShortyUrlRepository<ShortyEntity> _shortyUrlRepository, IShortCodeHistoryRepository<ShortyHistoryEntity> _shortCodeHistoryRepository) : IUrlService<ShortyEntity>
{
    public async Task<ShortyEntity> CreateShortAsync(string originalUrl, int userId)
    {

        if (string.IsNullOrEmpty(originalUrl))
        {
            throw new ArgumentNullException(nameof(originalUrl));
        }

        var shortUrl = Guid.NewGuid().ToString()[..6];

        if (await _shortyUrlRepository.ExistsByUrlOrShortAsync(originalUrl, shortUrl))
        {
            throw new ArgumentException("Url is exist");
        }

        var shorty = new ShortyEntity
        {
            CreatedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.Add(TimeSpan.FromDays(101)),
            ShortCode = shortUrl,
            IsActive = true,
            Url = originalUrl,
            UserId = userId,
        };
        await _shortyUrlRepository.AddShortyAsync(shorty);

        var history = new ShortyHistoryEntity
        {
            ShortyId = shorty.Id,
            ChangedAt = DateTime.UtcNow,
            NewShortUrl = "-",
            OldShortUrl = shortUrl
        };
        await _shortCodeHistoryRepository.AddHistoryAsync(history);
        return shorty;
    }

    public Task<bool> DeactivateByIdAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ShortyEntity>> GetAllShortCodesAsync()
    {
        var list = await _shortyUrlRepository.GetAllShortyAsync();
        if (list == null || !list.Any())
            throw new InvalidOperationException("Short codes list is empty.");
        return list;
    }

    public async Task<string> GetOriginalUrlAsync(string shortCode)
    {

        if (string.IsNullOrWhiteSpace(shortCode))
            throw new ArgumentException("ShortCode is empty.", nameof(shortCode));

        if (!await _shortyUrlRepository.IsShortCodeValidAsync(shortCode))
            throw new InvalidOperationException("ShortCode is not valid or expired.");

        var url = await _shortyUrlRepository.GetOriginalShortUrlAsync(shortCode);

        return url;
    }

    public async Task UpdateShortCodAsync(int id, string newShortyCode)
    {
        var entity = await _shortyUrlRepository.GetByIDAsync(id);

        if (id == 0)
        {
            throw new ArgumentNullException($"Id '{nameof(id)}' is empty");
        }

        if (string.IsNullOrWhiteSpace(newShortyCode))
        {
            throw new ArgumentException("Value cannot be empty.", nameof(newShortyCode));
        }

        if (!await _shortyUrlRepository.IsShortCodeValidAsync(entity.ShortCode))
            throw new InvalidOperationException("ShortCode is not valid or expired.");

        var newShortCode = new ShortyEntity
        {
            Id = id,
            UpdatedAt = DateTime.UtcNow,
            ShortCode = newShortyCode,
        };

        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(entity)} is empty");
        }

        var shorty = entity.ShortCode;

        var history = new ShortyHistoryEntity
        {
            ShortyId = id,
            ChangedAt = DateTime.UtcNow,
            NewShortUrl = newShortyCode,
            OldShortUrl = shorty
        };

        if (await _shortyUrlRepository.ExistsId(id))
        {
            await _shortyUrlRepository.UpdateAsync(newShortCode);
            await _shortCodeHistoryRepository.AddHistoryAsync(history);
        }
        else
        {
            throw new ArgumentNullException($"Id '{nameof(id)}' is not found");
        }
    }
    //public async Task<bool> DeleteByIdAsync(int id)
    //{
    //    //var rowsDeleted = await context.Shorties
    //    //    .Where(s => s.Id == id)
    //    //    .ExecuteDeleteAsync();

    //    //return rowsDeleted != 0;
    //}
    //public async Task<bool> DeleteByEmailAsync(string email)
    //{
    //    //var rowsDeleted = await context.Shorties
    //    //       .Where(s => s.User.Email == email)
    //    //       .ExecuteDeleteAsync();

    //    //return rowsDeleted != 0;
    //}
    //public async Task<bool> DeactivateByIdAsync(int id)
    //{
    //    //var rowsUpdated = await context.Shorties
    //    //      .ExecuteUpdateAsync(s => s.SetProperty(s => s.IsActive, false));

    //    //return rowsUpdated != 0;
    //}

}
