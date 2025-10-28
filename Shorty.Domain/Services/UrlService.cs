using Shorty.Dal.Db;
using Shorty.Dal.DbModel.Repositories;
using Shorty.Dal.Models;

namespace Shorty.Services
{
    public class UrlService
    {
        private IShortyUrlRepository<ShortyModel> _shortyUrlRepository;
        
        public UrlService(IShortyUrlRepository<ShortyModel> shortyUrlRepository)
        {
            _shortyUrlRepository = shortyUrlRepository;
        }

        public async Task<ShortyModel> CreateShortAsynq(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                throw new ArgumentNullException(nameof(originalUrl));
            }
            var shortOriginalUrl = "new short";

            var shorties = await _shortyUrlRepository.GetAllShortyAsync();
            // TODO db call
            var isExistUrl = shorties.Any(x => x.Url == originalUrl || x.ShortUrl == shortOriginalUrl);
            if (isExistUrl)
            {
                Console.WriteLine("Url or shortUrl already exists");
            }



            var shorty = new ShortyModel
            {
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                ShortUrl = shortOriginalUrl,
                Url = originalUrl,
                UserId = Guid.NewGuid(),


            };

            await _shortyUrlRepository.AddShortyAsync(shorty);
            
            return shorty;

        }



    }
}
