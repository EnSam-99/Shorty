using Shorty.Dal.DbModel.Repositories;
using Shorty.Dal.Entities;
using Shorty.Dal.Models;
using Shorty.Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Services.IServices
{
    public class ShortCodeHistoryService : IShortCodeHistoryService
    {
        private readonly IShortCodeHistoryRepository<ShortyHistoryModel> _repository;


        public ShortCodeHistoryService(IShortCodeHistoryRepository<ShortyHistoryModel> repository)
        {
            _repository = repository;
        }


        public async Task CreateHistoryAsync(Guid id, string? oldShortCode, string newShortCode)
        {
            if (string.IsNullOrWhiteSpace(oldShortCode))
                throw new ArgumentException("Value cannot be empty.", nameof(oldShortCode));
            if (string.IsNullOrWhiteSpace(newShortCode))
                throw new ArgumentException("Value cannot be empty.", nameof(newShortCode));
            if (oldShortCode == newShortCode)
                throw new ValidationException("New short code must differ from the old one.");

            if (!await _repository.ExsistOldShortCodeAsync(oldShortCode))
                throw new InvalidOperationException($"Old short code '{oldShortCode}' not found in current context.");

            if (await _repository.ExsistNewShortCodeAsync(newShortCode))
                throw new InvalidOperationException($"New short code '{newShortCode}' is already present.");

            var history = new ShortyHistoryModel
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
                        ?? Enumerable.Empty<ShortyHistoryModel>();

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
