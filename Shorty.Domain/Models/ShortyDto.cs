using Shorty.Dal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shorty.Models;

public class ShortyDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public string ShortUrl { get; set; } = default!;   
    public Guid UserId { get; set; }

}
