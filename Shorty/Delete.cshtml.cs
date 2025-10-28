using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Shorty
{
	public class DeleteModel : PageModel
	{
		private readonly Shorty.Dal.Data.Entities.ApplicationDBContext _context;

		public DeleteModel(Shorty.Dal.Data.Entities.ApplicationDBContext context)
		{
			_context = context;
		}

		[BindProperty]
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

		public async Task<IActionResult> OnPostAsync(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var shorty = await _context.Shorties.FindAsync(id);
			if (shorty != null)
			{
				Shorty = shorty;
				_context.Shorties.Remove(Shorty);
				await _context.SaveChangesAsync();
			}

			return RedirectToPage("./Index");
		}
	}
}
