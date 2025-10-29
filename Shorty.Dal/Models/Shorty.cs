using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shorty.Dal.Models;

public class Shorty
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
    
}