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
    public Task DeleteAsync(Guid id);
    public Task UpdateAsync(Guid id, T shortyModel);
    public Task GetByIDAsync(Guid id);
    public Task<List<T>> GetAllShortyAsync();
    


}
