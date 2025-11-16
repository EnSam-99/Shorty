namespace Shorty.Models;

public class ShortyDto
{
    public int Id { get; set; }
    public string Url { get; set; } = default!;
    public string ShortUrl { get; set; } = default!;
    public int UserId { get; set; }
    public bool IsActive { get; set; }
    public int Clicks { get; set; }
    public DateTime ScoreUpdateAt { get; set; }
    public DateTime LuastClickAt { get; set; }
}
