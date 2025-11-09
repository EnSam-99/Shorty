using Shorty.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Repositories.Abstractions;

public interface IVisitRepository
{
    public Task AddVisitAsync(VisitEntity entity);
}
