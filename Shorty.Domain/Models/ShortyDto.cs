using Shorty.Dal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shorty.Models;

public class ShortyDto
{
    public int Id { get; set; }
    public string Url { get; set; } = default!;
    public string ShortUrl { get; set; } = default!;   
    public int UserId { get; set; }
}
