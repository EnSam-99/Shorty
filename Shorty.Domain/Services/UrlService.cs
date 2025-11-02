using Shorty.Dal.Db;
using Shorty.Dal.DbModel.Repositories;
using Shorty.Dal.Entities;
using Shorty.Dal.Models;
using Shorty.Services.IServices;
using System;
using System.Collections.Generic;

namespace Shorty.Services
{
    public class UrlService: IUrlService<ShortyModel>
    {
        private IShortyUrlRepository<ShortyModel> _shortyUrlRepository;
        private IShortCodeHistoryRepository<ShortyHistoryModel> _shortCodeHistoryRepository;

        public UrlService(IShortyUrlRepository<ShortyModel> shortyUrlRepository, IShortCodeHistoryRepository<ShortyHistoryModel> shortCodeHistoryRepository)
        {
            _shortyUrlRepository = shortyUrlRepository;
            _shortCodeHistoryRepository = shortCodeHistoryRepository;
        }

        public async Task<ShortyModel> CreateShortAsync(string originalUrl, Guid userId)
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
            ;

            var shorty = new ShortyModel
            {
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                ShortCode = shortUrl,
                Url = originalUrl,
                UserId = userId,
            };

            await _shortyUrlRepository.AddShortyAsync(shorty);

            var history = new ShortyHistoryModel
            {
                ShortyId = shorty.Id,
                ChangedAt = DateTime.UtcNow,
                NewShortUrl = "-",
                OldShortUrl = shortUrl
            };
            await _shortCodeHistoryRepository.AddHistoryAsync(history);


            return shorty;

        }

        public async Task<IEnumerable<ShortyModel>> GetAllShortCodesAsync()
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

        public async Task UpdateShortCodAsync(Guid id, string newShortyCode)
        {
            if (id == Guid.Empty) 
            {
                throw new ArgumentNullException($"Id '{nameof(id)}' is empty");
            }
            if (string.IsNullOrWhiteSpace(newShortyCode))
            {
                throw new ArgumentException("Value cannot be empty.", nameof(newShortyCode));
            }

            var newShortCode = new ShortyModel
            {
                Id = id,
                LastAccessedAt = DateTime.UtcNow,
               ShortCode = newShortyCode,

            };

            var entity = await _shortyUrlRepository.GetByIDAsync(id);
            if(entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} is empty");
            }
            var shorty = entity.ShortCode;

            var history = new ShortyHistoryModel
            {
                ShortyId = id,
                ChangedAt = DateTime.UtcNow,
                NewShortUrl = newShortyCode,
                OldShortUrl = shorty
            };

            if (await _shortyUrlRepository.ExsistsId(id))
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
