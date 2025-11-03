namespace Shorty.Domain.Models.Response
{
	public class ShortyListDto
	{
		public int Id { get; set; }
		public string ShortyUrl { get; set; } = string.Empty;
		public string OriginalUrl { get; set; } = string.Empty;
		public DateTime CreatedDate { get; set; }
	}
}
