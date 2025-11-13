using Microsoft.EntityFrameworkCore;
using Shorty.Dal;

public class ShortyStatusService(AppDbContext dbContext)
{
    public async Task UpdateShortiesStatusAsync()
    {
        var now = DateTime.UtcNow;

        var allShorties = await dbContext.Shorties.ToListAsync();
   
        foreach (var shorty in allShorties)
        { 
            shorty.IsActive = shorty.ExpiredAt > now;
        }

        await dbContext.SaveChangesAsync();
    }

    
}