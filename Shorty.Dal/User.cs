using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Shorty.Dal
{
	[Index(nameof(Email), IsUnique = true)]
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required, EmailAddress, StringLength(320)]
		public string Email { get; set; } = string.Empty;

		public ICollection<ShortLink> Shorties { get; set; } = new List<ShortLink>();

	}
}
