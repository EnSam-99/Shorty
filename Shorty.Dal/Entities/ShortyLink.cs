using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shorty.Dal.Entities;

public class ShortyLink
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public string ShortUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public ICollection<Visit> Visits = new List<Visit>();
    public bool IsActive { get; set; } = true;
    public DateTime? DeletedAt { get; set; }

}