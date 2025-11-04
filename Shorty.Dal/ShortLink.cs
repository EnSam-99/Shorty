using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal
{
	[Index(nameof(ShortyUrl), IsUnique = true)]
	public class ShortLink
    {
        public int Id { get; set; }

		[Required, StringLength(2048)]
		public string Url { get; set; } = string.Empty;

		[Required, StringLength(6)]
		public string ShortyUrl { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

		public int Clicks { get; set; } = 0;
		public DateTime? LastAccessed { get; set; }

		[Required]
		public int UserId { get; set; }
        public User? User { get; set; }
    }
}
