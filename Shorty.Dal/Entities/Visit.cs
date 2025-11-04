namespace Shorty.Dal.Entities;

public class Visit
{
    public int Id { get; set; }
    public int ShortyId { get; set; } 
    public ShortyLink ShortyLink { get; set; } 
    public DateTime VisitedAt { get; set; }
}