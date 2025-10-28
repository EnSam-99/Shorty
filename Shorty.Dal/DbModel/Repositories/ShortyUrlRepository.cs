using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Db;
using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.DbModel.Repositories;

public class ShortyUrlRepository: IShortyUrlRepository<ShortyModel>
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

    public Task DeleteAsync(ShortyModel shortyid)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ShortyModel>> GetAllShortyAsync()
    {
        return await _context.Shorties.OrderByDescending(s => s.Id).ToListAsync();
    }

    public Task GetByIDAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid id, ShortyModel shortyModel)
    {
        throw new NotImplementedException();
    }
}