using Microsoft.EntityFrameworkCore;
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

        public AnalyticsService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<TopShortLinkDto>> GetTopShortLinksAsync(string email)
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
    }
}
