using System.ComponentModel.DataAnnotations;

namespace Shorty.Dal.Models;

public class UserEntity
{
    public int Id { get; set; } = default!;
    [Required]
    public string Name { get; set; } = default!;
    [Required]
    public string Email { get; set; } = default!;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public List<ShortyEntity> Shorties { get; set; } = new();
}
