using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Db;
using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.DbModel.Repositories;

public class ShortyUrlRepository : IShortyUrlRepository<ShortyModel>
{
    readonly AppDbContext _context;

    public ShortyUrlRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddShortyAsync(ShortyModel shortyModel)
    {

        await _context.Shorties.AddAsync(shortyModel);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsByUrlOrShortAsync(string origonalUrl, string shortUrl) =>
        _context.Shorties.AnyAsync(u => u.Url.Trim() == origonalUrl.Trim()
        || u.ShortCode.Trim() == shortUrl.Trim());

    public async Task<bool> ExsistsId(Guid id)
    {
     var isExists = await  _context.Shorties.AnyAsync(u => u.Id == id);
        return isExists;
    }
    

    public async Task<List<ShortyModel>> GetAllShortyAsync()
    {
        var list = await _context.Shorties.OrderByDescending(s => s.Id).ToListAsync();
        return list;
    }

    public async Task<ShortyModel> GetByIDAsync(Guid id)
    {
        var shorty = await _context.Shorties.FirstOrDefaultAsync(s => s.Id == id);
        return shorty;
    }

    public async Task UpdateAsync( ShortyModel shortyModel)
    {
        if (shortyModel is null)
            throw new ArgumentNullException(nameof(shortyModel));
        if (shortyModel.Id == Guid.Empty)
            throw new ArgumentException("Id is empty.", nameof(shortyModel.Id));
        if (string.IsNullOrWhiteSpace(shortyModel.ShortCode))
            throw new ArgumentException("ShortCode is empty.", nameof(shortyModel.ShortCode));

        await _context.Shorties.Where(x => x.Id == shortyModel.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.ShortCode, shortyModel.ShortCode)         
        .SetProperty(l=>l.LastAccessedAt, DateTime.UtcNow));

        
    }


}