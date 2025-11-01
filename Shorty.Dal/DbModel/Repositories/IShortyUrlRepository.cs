using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.DbModel.Repositories;

public interface IShortyUrlRepository<T>
{
    public Task AddShortyAsync(T shortyModel);   
    public Task UpdateAsync(ShortyModel newShortCode);
    public Task<T> GetByIDAsync(Guid id);
    public Task<List<T>> GetAllShortyAsync();

    public Task<bool> ExistsByUrlOrShortAsync(string origonalUrl, string shortUrl);
   public Task<bool> ExsistsId(Guid id);
}
