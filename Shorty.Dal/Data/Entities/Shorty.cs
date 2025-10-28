using System.ComponentModel.DataAnnotations;

namespace Shorty.Dal.Data.Entities;

public class Shorty
{
	public Guid ID { get; set; }
	[Url]
	[Required]
	[MaxLength(2048)]
	public string OriginalUrl { get; set; } = string.Empty;

	[MaxLength(10)]
	public string ShortUrl { get; set; } = string.Empty;
	public DateTime CreatedAt { get; }	= DateTime.UtcNow;
	public Guid UserID { get; set; }
	public User User { get; set; } = null!;

}
