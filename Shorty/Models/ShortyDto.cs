using Shorty.Dal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shorty.Models;

public class ShortyDto
{
    public string Url { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;   
    public Guid UserId { get; set; }

}
