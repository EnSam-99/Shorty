using System.ComponentModel.DataAnnotations;

namespace Shorty.Dal.Data.Entities;

public class User
{
	public Guid ID { get; set; }
	[Required]
	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;
	[Required]
	[MaxLength(255)]
	public string Email { get; set; } = string.Empty;
	public DateTime CreatedAt { get; }	= DateTime.UtcNow;
	public List<Shorty> Shorties { get; set; } = new List<Shorty>();
}
