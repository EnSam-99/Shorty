using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Domain.Abstraction;
using Shorty.Domain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Services
{
    public class ShortyStatisticsService(AppDbContext _context) : IShortyStatisticsService
    {
        public async Task<IEnumerable<ShortyStatDto>> GetStatisticsAsync()
        {
            var shorties = await _context.Shorties
                .Include(s => s.Visits)
                .ToListAsync();

            var stats = shorties
                .Select(s => new
                {
                    CleanUrl = s.Url.Normalize(),
                    Visits = s.Visits
                })
                .GroupBy(x => x.CleanUrl)
                .Select(g => new ShortyStatDto
                {
                    Url = g.Key,
                    Clicks = g.Sum(x => x.Visits.Count),
                    LastAccessedDate = g.SelectMany(x => x.Visits)
                                    .Max(v => (DateTime?)v.CreatedDate)
                })
                .OrderByDescending(s => s.LastAccessedDate)
                .ToList();

            return stats;
        }

        public async Task AddVisitAsync(string shortyUrl)
        {
            var shortLink = await _context.Shorties
                .Include(s => s.Visits)
                .FirstOrDefaultAsync(s => s.ShortyUrl == shortyUrl);

            if (shortLink != null)
            {
                var lastVisit = shortLink.Visits
                    .OrderByDescending(v => v.CreatedDate)
                    .FirstOrDefault();

                if (lastVisit != null && (DateTime.UtcNow - lastVisit.CreatedDate).TotalSeconds < 2)
                    return;

                var visit = new Visit { ShortLinkId = shortLink.Id, CreatedDate = DateTime.UtcNow };
                _context.Visits.Add(visit);

                await _context.SaveChangesAsync();
            }
        }

    }
}

