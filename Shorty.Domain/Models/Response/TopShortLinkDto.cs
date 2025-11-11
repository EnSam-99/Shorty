namespace Shorty.Domain.Models.Response;

public class TopShortLinkDto
{
    public string ShortUrl { get; set; }
    public string OriginalUrl { get; set; }
    public long Clicks { get; set; }

	public decimal Score { get; set; }

	public DateTime LastAccessedDate { get; set; }
}
