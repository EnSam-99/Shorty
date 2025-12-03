namespace Shorty.Domain.Models.Response
{
    public class ShortyStatDto
    {
       
        public string Url { get; set; } = string.Empty;
        public string ShortyUrl { get; set; } = string.Empty;
        public int Clicks { get; set; }
        public DateTime? LastAccessedDate { get; set; }
        public double Score { get; set; }
    }
}
