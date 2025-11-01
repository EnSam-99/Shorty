using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Models;

public class ShortyModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();  
    [Required, MaxLength(2048)]
    public string Url { get; set; } = string.Empty; 
    [Required, MaxLength(64)]
    public string ShortCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public long Clicks { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime? LastAccessedAt { get; set; }
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public User? User { get; set; }
}