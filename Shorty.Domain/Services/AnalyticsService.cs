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
			List<ShortLink> shortLinks = await dbContext.Shorties.Include(s => s.Visits).ToListAsync();

			foreach (var shortLink in shortLinks)
			{
				decimal totalDays = (decimal)(DateTime.UtcNow - shortLink.CreatedDate).TotalDays;

				shortLink.Score = shortLink.Visits.Count / (totalDays != 0 ? totalDays : 1)
					+ (shortLink.IsActive ? 10 : 0);
			}

			await dbContext.SaveChangesAsync();
		}
	}
}
