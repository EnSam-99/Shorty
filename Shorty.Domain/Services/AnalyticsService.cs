using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shorty.Dal;
using Shorty.Dal.Entities;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Shorty.Domain.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly AppDbContext dbContext;
		private readonly IServiceProvider serviceProvider;

		public AnalyticsService(AppDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = dbContext;
			this.serviceProvider = serviceProvider;
		}

        public async Task<List<TopShortLinkDto>> GetTopShortLinkAsync(string email)
        {
            return await dbContext.Shorties
                .Where(s => s.User.Email == email)
                .Select(s => new TopShortLinkDto
                {
                    ShortUrl = s.ShortyUrl,
                    OriginalUrl = s.Url,
                    Clicks = s.Visits.Count()
                })
                .OrderByDescending(s => s.Clicks)
                .Take(10)
                .ToListAsync();
        }

		public async Task<List<TopShortLinkDto>> GetTopShortLinksAsync()
		{
			return await dbContext.Shorties
				.Select(s => new TopShortLinkDto
				{
					Score = s.Score,
					ShortUrl = s.ShortyUrl,
					OriginalUrl = s.Url,
					Clicks = s.Visits.Count()
				})
				.OrderByDescending(s => s.Score)
				.Take(10)
				.ToListAsync();
		}

		public async Task RecalculateScoresAsync()
		{
			var shortLinks = await dbContext.Shorties.Include(s => s.Visits).ToListAsync();

			foreach (var shortLink in shortLinks)
			{

				shortLink.Score = shortLink.IsActive ? shortLink.Visits.Count / (decimal)((DateTime.UtcNow - shortLink.CreatedDate).TotalDays + 1) + 10 : 0;
				
			}

			await dbContext.SaveChangesAsync();
		}

		//public async Task RecalculateScoresAsync()
		//{
		//	const int batchSize = 2;

		//	var shortyIds = await dbContext.Shorties
		//		.OrderBy(s => s.Id)
		//		.Select(s => s.Id)
		//		.ToListAsync();

		//	await Parallel.ForEachAsync(
		//		shortyIds.Chunk(batchSize),
		//		new ParallelOptions { MaxDegreeOfParallelism = 4 },
		//		async (idBatch, token) =>
		//		{
		//			using var scope = serviceProvider.CreateScope();
		//			var scopedDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		//			var shorties = await scopedDb.Shorties
		//				.Include(s => s.Visits)
		//				.Where(s => idBatch.Contains(s.Id))
		//				.ToListAsync(token);

		//			foreach (var s in shorties)
		//			{
		//				var ageInDays = (DateTime.UtcNow - s.CreatedDate).TotalDays + 1;
		//				s.Score = s.IsActive
		//					? s.Visits.Count / (decimal)ageInDays + 10
		//					: 0;
		//			}

		//			await scopedDb.SaveChangesAsync(token);
		//		});
		//}

	}
}
