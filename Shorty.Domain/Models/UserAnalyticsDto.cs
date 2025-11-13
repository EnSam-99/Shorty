using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Models
{
    public record MostPopularDto(string ShortCode, int Clicks);
    public record UserAnalyticsDto(
        int TotalShortenedUrls,
        int TotalClicks,
        decimal AverageClicksPerShorty,
        string? MostPopularShortCode,
        MostPopularDto? MostPopularClicks
    );
}
