namespace Shorty.Dal.Entities;

public class VisitEntity
{
    public int Id { get; set; }
    public int ShortyId { get; set; }
    public ShortyEntity Shorty { get; set; }
    public DateTime CreatedDate { get; set; }
}
