using Microsoft.EntityFrameworkCore;
using Shorty.Dal;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Domain.Services;

public class DeletionUrlService(AppDbContext context) : IDeletionUrlService
{
    public async Task<bool> DeleteByIdAsync(int id)
    {
        var rowsDeleted = await context.Shorties
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();

        return rowsDeleted != 0;
    }

    public async Task<bool> DeleteByEmailAsync(string email)
    {
        var rowsDeleted = await context.Shorties
               .Where(s => s.User.Email == email)
               .ExecuteDeleteAsync();

        return rowsDeleted != 0;
    }

    public async Task<bool> DeactivateByIdAsync(int id)
    {
        var rowsUpdated = await context.Shorties
              .ExecuteUpdateAsync(s => s.SetProperty(s => s.IsActive, false));

        return rowsUpdated != 0;
    }
}
