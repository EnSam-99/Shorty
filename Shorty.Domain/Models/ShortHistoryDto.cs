using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Models;

public class ShortHistoryDto
{
    public int ShortyId { get; set; } 
    public string? OldShort { get; set; } 
    public string NewShort { get; set; } = null!;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
