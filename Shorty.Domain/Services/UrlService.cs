using Shorty.Dal.Db;
using Shorty.Dal.Db.IRepositories;
using Shorty.Dal.Entities;
using Shorty.Dal.Models;
using Shorty.Services.IServices;
using System;
using System.Collections.Generic;

namespace Shorty.Services
{
    public class UrlService: IUrlService<ShortyEntity>
    {
        private IShortyUrlRepository<ShortyEntity> _shortyUrlRepository;
        private IShortCodeHistoryRepository<ShortyHistoryEntity> _shortCodeHistoryRepository;

        public UrlService(IShortyUrlRepository<ShortyEntity> shortyUrlRepository, IShortCodeHistoryRepository<ShortyHistoryEntity> shortCodeHistoryRepository)
        {
            _shortyUrlRepository = shortyUrlRepository;
            _shortCodeHistoryRepository = shortCodeHistoryRepository;
        }
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
                ExpiredAt = DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)),
                ShortCode = shortUrl,
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

          var url =  await _shortyUrlRepository.GetOriginalShortUrlAsync(shortCode);

            return url;
        }

        public async Task UpdateShortCodAsync(int id, string newShortyCode)
        {
            if (id == 0) 
            {
                throw new ArgumentNullException($"Id '{nameof(id)}' is empty");
            }
            if (string.IsNullOrWhiteSpace(newShortyCode))
            {
                throw new ArgumentException("Value cannot be empty.", nameof(newShortyCode));
            }

            var newShortCode = new ShortyEntity
            {
                Id = id,
                UpdatedAt = DateTime.UtcNow,
               ShortCode = newShortyCode,

            };

            var entity = await _shortyUrlRepository.GetByIDAsync(id);
            if(entity == null)
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
    }
}
