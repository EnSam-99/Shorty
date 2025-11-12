using Shorty.Dal.Repositories.Abstractions;
using Shorty.Domain.Models;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services;
using Shorty.Dal.Entities;

public class ClickDetailService(IClickDetailRepository _clickDetailRepository) : IClickDetailService
{
    public async Task<ClickDetailDto> GetClickSummaryAsync(int shortyId)
    {
        var clicks = await _clickDetailRepository.GetByShortyIdAsync(shortyId);

        var uniqueIps = clicks.Select(c => c.IpAddress).Distinct().Count();
        var totalClicks = clicks.Count();
        var avgVisitsPerIp = uniqueIps > 0 ? Math.Round((decimal)totalClicks / uniqueIps, 2) : 0m;

        return new ClickDetailDto
        {
            IpAddress = uniqueIps,
            ShortyCount = totalClicks,
            VisitCount = avgVisitsPerIp
        };
    }
    
    public async Task  AddClickAsync(int shortyId, string ipAddress)
    {
        var click = new ClickDetailEntity
        {
            ShortyId = shortyId,
            IpAddress = ipAddress,
            AccessedAt = DateTime.UtcNow
        };

        await _clickDetailRepository.AddAsync(click);
    }
}