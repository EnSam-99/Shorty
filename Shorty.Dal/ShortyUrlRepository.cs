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



}
