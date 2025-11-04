using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Models;

public class UpdateShortRequestDto
{
    public int ShortyId { get; set; }
    public string NewShort { get; set; } = default!;
}
