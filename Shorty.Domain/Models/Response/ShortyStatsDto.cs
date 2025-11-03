namespace Shorty.Domain.Models.Response
{
	public class ShortyStatsDto
	{
		public int ShortId { get; set; }
		public int Clicks { get; set; }
		public DateTime? LastAccessed { get; set; }
	}
}
