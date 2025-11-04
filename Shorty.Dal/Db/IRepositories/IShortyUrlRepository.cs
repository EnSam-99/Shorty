using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Db.IRepositories;

public interface IShortyUrlRepository<T>
{
    public Task AddShortyAsync(T shortyModel);   
    public Task UpdateAsync(ShortyEntity newShortCode);
    public Task<T> GetByIDAsync(int id);
    public Task<List<T>> GetAllShortyAsync();
    public Task<bool> ExistsByUrlOrShortAsync(string origonalUrl, string shortUrl);
    public Task<bool> ExistsId(int id);
    public Task<string> GetOriginalShortUrlAsync(string shortCode);
}
