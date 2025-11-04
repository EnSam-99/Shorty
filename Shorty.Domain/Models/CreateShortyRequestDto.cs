using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Models;

public class CreateShortyRequestDto
{
    public string Url { get; set; } = default!;
    public int UserId { get; set; }
}
