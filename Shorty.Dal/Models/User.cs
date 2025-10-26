using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

   public List<ShortyModel> Shorties { get; set; } = new();
}
