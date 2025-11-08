using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Entities;
using Shorty.Domain.Models;
using Shorty.Domain.Services.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Shorty.Domain.Services
{
    public class ShortCodeHistoryService : IShortCodeHistoryService
    {
        private readonly IShortCodeHistoryRepository<ShortyHistoryEntity> _repository;
        public ShortCodeHistoryService(IShortCodeHistoryRepository<ShortyHistoryEntity> repository)
        {
            _repository = repository;
        }
        public async Task CreateHistoryAsync(int id, string? oldShortCode, string newShortCode)
        {
            if (string.IsNullOrWhiteSpace(oldShortCode))
                throw new ArgumentException("Value cannot be empty.", nameof(oldShortCode));

            if (string.IsNullOrWhiteSpace(newShortCode))
                throw new ArgumentException("Value cannot be empty.", nameof(newShortCode));

            if (oldShortCode == newShortCode)
                throw new ValidationException("New short code must differ from the old one.");

            if (!await _repository.ExistsOldShortCodeAsync(oldShortCode))
                throw new InvalidOperationException($"Old short code '{oldShortCode}' not found in current context.");

            if (await _repository.ExistsNewShortCodeAsync(newShortCode))
                throw new InvalidOperationException($"New short code '{newShortCode}' is already present.");

            var history = new ShortyHistoryEntity
            {
                ShortyId = id,
                OldShortUrl = oldShortCode,
                NewShortUrl = newShortCode,
                ChangedAt = DateTime.UtcNow
            };

            await _repository.AddHistoryAsync(history);
        }
        public async Task<IEnumerable<ShortHistoryDto>> GetAllShortsHistoryAsync()
        {
            var items = await _repository.GetAllHistoryAsync()
                        ?? Enumerable.Empty<ShortyHistoryEntity>();

            return items.Select(x => new ShortHistoryDto
            {
                ShortyId = x.ShortyId,
                OldShort = x.OldShortUrl,
                NewShort = x.NewShortUrl,
                ChangedAt = x.ChangedAt
            });
        }
    }
}
