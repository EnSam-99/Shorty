namespace Shorty.Domain.Models.Response;

public class TopShortLinkDto
{
    public string ShortCode { get; set; }
    public string Url { get; set; }
    public decimal Score { get; set; }
    public long Clicks { get; set; }
    public DateTime LastClickedAt { get; set; }
}
