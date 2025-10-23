using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Models;

public class Shorty
{
    public Guid Id { get; set; }
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public string ShortUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; } = DateTime.UtcNow.Date;

    [ForeignKey("User")]
    public Guid UserId { get; set; }
}