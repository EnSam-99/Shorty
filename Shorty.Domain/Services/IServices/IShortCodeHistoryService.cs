using Shorty.Dal.Db.Repositories;
using Shorty.Dal.Entities;
using Shorty.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Services.IServices;

public interface IShortCodeHistoryService
{
    public Task CreateHistoryAsync(int id, string? oldShortCode, string newShortCode);
    public Task<IEnumerable<ShortHistoryDto>> GetAllShortsHistoryAsync();
}
