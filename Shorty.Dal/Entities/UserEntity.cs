using System.ComponentModel.DataAnnotations;

namespace Shorty.Dal.Entities;

public class UserEntity
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    public DateTime CreatedAt { get; }
    public List<ShortyEntity> Shorties { get; set; }
}
