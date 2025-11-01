using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Db;
using Shorty.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.DbModel.Repositories;

public class ShortCodeHistoryRepository : IShortCodeHistoryRepository<ShortyHistoryModel>
{
    private readonly AppDbContext _context;
    public ShortCodeHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddHistoryAsync(ShortyHistoryModel history)
    {
      await  _context.Histories.AddAsync(history);
      await  _context.SaveChangesAsync();
    }

    public async Task<bool> ExsistNewShortCodeAsync(string newShortCode)=>   
      await _context.Histories.AnyAsync(n=> n.NewShortUrl.Trim() == newShortCode.Trim()); 
    

    public async Task<bool> ExsistOldShortCodeAsync(string oldShortCode)=>   
        await _context.Histories.AnyAsync(n => n.OldShortUrl.Trim() == oldShortCode.Trim());

    public async Task<IEnumerable<ShortyHistoryModel>> GetAllHistoryAsync()
    {
       
       var historyList = await _context.Histories.OrderByDescending(x=>x.Id).ToListAsync();

        return historyList;

    }
}
