using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Shorty
{
	public class EditModel : PageModel
	{
		private readonly Shorty.Dal.Data.Entities.ApplicationDBContext _context;

		public EditModel(Shorty.Dal.Data.Entities.ApplicationDBContext context)
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
			Shorty = shorty;
			ViewData["UserID"] = new SelectList(_context.Users, "ID", "Email");
			return Page();
		}

		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more information, see https://aka.ms/RazorPagesCRUD.
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			_context.Attach(Shorty).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ShortyExists(Shorty.ID))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return RedirectToPage("./Index");
		}

		private bool ShortyExists(Guid id)
		{
			return _context.Shorties.Any(e => e.ID == id);
		}
	}
}
