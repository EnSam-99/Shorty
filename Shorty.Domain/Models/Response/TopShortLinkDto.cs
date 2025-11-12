namespace Shorty.Domain.Models.Response;

public class TopShortLinkDto
{
    public string ShortCode { get; set; }
    public string Url { get; set; }
    public long Score { get; set; }
    public DateTime LastClickedAt { get; set; }
}
