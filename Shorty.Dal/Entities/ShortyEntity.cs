using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shorty.Dal.Models;

public class ShortyEntity
{
    [Key]
    public int Id { get; set; } = default!;
    [Required, MaxLength(2048)]
    public string Url { get; set; } = default!;
    [Required, MaxLength(64)]
    public string ShortCode { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiredAt { get; set; }
    public long Clicks { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public decimal Score { get; set; } = 0m;
    public DateTime? UpdatedAt { get; set; }
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public UserEntity? User { get; set; }
}