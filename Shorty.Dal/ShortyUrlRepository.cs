using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal;

public class ShortyUrlRepository
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
   
    public async Task<List<ShortyModel>> GetAllShortyAsync()
    {
        return await _context.Shorties.OrderByDescending(s => s.Id).ToListAsync();
    }

}
