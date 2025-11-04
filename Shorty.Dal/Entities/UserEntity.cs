using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
