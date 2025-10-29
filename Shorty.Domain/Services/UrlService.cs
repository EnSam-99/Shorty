using Shorty.Dal.Db;
using Shorty.Dal.DbModel.Repositories;
using Shorty.Dal.Models;
using Shorty.Services.IServices;

namespace Shorty.Services
{
    public class UrlService: IUrlService
    {
        private IShortyUrlRepository<ShortyModel> _shortyUrlRepository;

        public UrlService(IShortyUrlRepository<ShortyModel> shortyUrlRepository)
        {
            _shortyUrlRepository = shortyUrlRepository;
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
                ShortUrl = shortUrl,
                Url = originalUrl,
                UserId = userId,
            };

            await _shortyUrlRepository.AddShortyAsync(shorty);

            return shorty;

        }



    }
}
