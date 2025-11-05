using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Services
{
    public class ShortyStatisticsService
    {

        private readonly AppDbContext _context;

        public ShortyStatisticsService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ShortyStatDto>> GetStatisticsAsync()
        {
            var shorties = await _context.Shorties
                .Include(s => s.Visits)
                .ToListAsync();

            var stats = shorties
                .Select(s => new
                {
                    CleanUrl = NormalizeUrl(s.Url),
                    Visits = s.Visits
                })
                .GroupBy(x => x.CleanUrl)
                .Select(g => new ShortyStatDto
                {
                    Url = g.Key,
                    Clicks = g.Sum(x => x.Visits.Count),
                    LastAccessed = g.SelectMany(x => x.Visits)
                                    .Max(v => (DateTime?)v.CreatedDate)
                })
                .OrderByDescending(s => s.LastAccessed)
                .ToList();

            return stats;
        }

        private static string NormalizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return url;

            url = url.ToLower().Trim();
            url = url.TrimEnd('/');          
            if (url.StartsWith("www."))
                url = url.Substring(4);      

            return url;
        }
        public async Task RecordVisitAsync(string shortyUrl)
        {
            var shortLink = await _context.Shorties
                .Include(s => s.Visits)
                .FirstOrDefaultAsync(s => s.ShortyUrl == shortyUrl);

            if (shortLink != null)
            {
                var lastVisit = shortLink.Visits
                    .OrderByDescending(v => v.CreatedDate)
                    .FirstOrDefault();

                // Ignore duplicate if last visit is within 2 seconds
                if (lastVisit != null && (DateTime.UtcNow - lastVisit.CreatedDate).TotalSeconds < 2)
                    return;

                var visit = new Visit { ShortLinkId = shortLink.Id, CreatedDate = DateTime.UtcNow };
                _context.Visits.Add(visit);

                await _context.SaveChangesAsync();
            }
        }

    }
}

