namespace Shorty.Domain.Models;

public class UpdateShortRequestDto
{
    public int ShortyId { get; set; }
    public string NewShort { get; set; } = default!;
}
