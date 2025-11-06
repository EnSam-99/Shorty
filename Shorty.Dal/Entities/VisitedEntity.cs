using Shorty.Dal.Models;

namespace Shorty.Dal.Entities;

public class VisitedEntity
{
    public int Id { get; set; }
    public int ShortLinkId { get; set; }
    public ShortyEntity Shorty { get; set; } = null!;
    public DateTime VisitDate { get; set; }
}
