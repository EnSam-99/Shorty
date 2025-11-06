namespace Shorty.Domain.Models;

public class CreateShortyRequestDto
{
    public string Url { get; set; } = default!;
    public int UserId { get; set; }
}
