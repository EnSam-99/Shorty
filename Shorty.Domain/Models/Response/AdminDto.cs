using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Models.Response;

public class AdminDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsAdmin { get; set; }
    public int LinksCount { get; set; }
    public long TotalClicks { get; set; }
    public int ActiveLinks { get; set; }
    public bool IsActive { get; set; } = true;
}
