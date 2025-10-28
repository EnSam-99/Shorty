using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Shorty
{
	public class DetailsModel : PageModel
	{
		private readonly Shorty.Dal.Data.Entities.ApplicationDBContext _context;

		public DetailsModel(Shorty.Dal.Data.Entities.ApplicationDBContext context)
		{
			_context = context;
		}

		public Shorty.Dal.Data.Entities.Shorty Shorty { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var shorty = await _context.Shorties.FirstOrDefaultAsync(m => m.ID == id);
			if (shorty == null)
			{
				return NotFound();
			}
			else
			{
				Shorty = shorty;
			}
			return Page();
		}
	}
}
