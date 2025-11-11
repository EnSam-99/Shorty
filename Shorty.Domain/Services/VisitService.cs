using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Dal.Entities;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services;

public class VisitService(AppDbContext context) : IVisitService
{
    public async Task AddVisitAsync(string shortCode)
    {
		try
		{ 
        var shorty = await context.Shorties
                .FirstOrDefaultAsync(s => s.ShortCode == shortCode);

        if (shorty == null)
        {
            return;
        }

        var visit = new VisitEntity { ShortyId = shorty.Id, CreatedDate = DateTime.UtcNow };
        await context.Visits.AddAsync(visit);


		ShortyClickCount clickCount = await context.ShortyClickCounts
								.FindAsync(shorty.Id);

		if (clickCount is null)
		{
			await context.ShortyClickCounts.AddAsync(new ShortyClickCount
			{
				ShortyId = shorty.Id,
				ClickCount = 1
			});
		}
		else
		{
			clickCount.ClickCount++;
			context.ShortyClickCounts.Update(clickCount);
		}

		await context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error in AddVisitAsync({shortCode}): {ex.Message}");
		}
	}
}
