using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Domain.Models.Response;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services
{
    public class ShortyPerformanceService(AppDbContext context) : IShortyPerformanceService
    {
        private readonly AppDbContext _context = context;

        //public async Task RecalculateScoresAsync()
        //{
        //    var now = DateTime.UtcNow;
        //    var shorties = await _context.Shorties
        //        .Include(s => s.Visits)
        //        .ToListAsync();

        //    foreach (var s in shorties)
        //    {
        //        var clicks = s.Visits.Count;
        //        var ageInDays = (now - s.CreatedAt).TotalDays;
        //        if (ageInDays < 1) ageInDays = 1;

        //        var score = (clicks / ageInDays) + (s.IsActive ? 10 : 0);
        //        s.Score = (decimal)score;
        //    }

        //    await _context.SaveChangesAsync();
        //}

        //public async Task<IEnumerable<ShortyStatDto>> GetTopPerformingShortiesAsync(int limit = 10)
        //{
        //    var now = DateTime.UtcNow;

        //    await RecalculateScoresAsync();

        //    return await _context.Shorties
        //        .OrderByDescending(s => s.Score)
        //        .Take(limit)
        //        .Select(s => new ShortyStatDto
        //        {
        //            Url = s.Url,
        //            ShortyUrl = s.Url,
        //            Clicks = s.Visits.Count,
        //            LastAccessedDate = s.Visits
        //                .OrderByDescending(v => v.CreatedDate)
        //                .Select(v => (DateTime?)v.CreatedDate)
        //                .FirstOrDefault(),
        //            Score = (double)s.Score
        //        })
        //        .ToListAsync();
        //}
        public async Task RecalculateScoresAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(@"
UPDATE ""Shorties"" s
SET ""Score"" = 
    (COALESCE(v.""Clicks"", 0)::double precision / GREATEST(EXTRACT(DAY FROM NOW() - s.""CreatedAt""), 1))
    + CASE WHEN s.""IsActive"" THEN 10 ELSE 0 END
FROM (
    SELECT ""ShortyId"", COUNT(*) AS ""Clicks""
    FROM ""Visits""
    GROUP BY ""ShortyId""
) v
WHERE s.""Id"" = v.""ShortyId"" OR v.""ShortyId"" IS NULL;
");
        }

        public async Task<IEnumerable<ShortyStatDto>> GetTopPerformingShortiesAsync(int limit = 10)
        {
            return await _context.Shorties
                .OrderByDescending(s => s.Score)
                .Take(limit)
                .Select(s => new ShortyStatDto
                {
                    Url = s.Url,
                    ShortyUrl = s.Url,
                    Clicks = s.Visits.Count,
                    LastAccessedDate = s.Visits
                        .OrderByDescending(v => v.CreatedDate)
                        .Select(v => (DateTime?)v.CreatedDate)
                        .FirstOrDefault(),
                    Score = (double)s.Score
                })
                .ToListAsync();
        }
    }
}
