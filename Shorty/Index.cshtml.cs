using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Data.Entities;

namespace Shorty
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public IndexModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public IList<Shorty.Dal.Data.Entities.Shorty> Shorty { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Shorty = await _context.Shorties
                .Include(s => s.User).ToListAsync();
        }
    }
}
