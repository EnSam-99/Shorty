using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shorty
{
	public class CreateModel : PageModel
	{
		private readonly Shorty.Dal.Data.Entities.ApplicationDBContext _context;

		public CreateModel(Shorty.Dal.Data.Entities.ApplicationDBContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
			ViewData["UserID"] = new SelectList(_context.Users, "ID", "Email");
			return Page();
		}

		[BindProperty]
		public Shorty.Dal.Data.Entities.Shorty Shorty { get; set; } = default!;

		// For more information, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.Shorties.Add(Shorty);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}
