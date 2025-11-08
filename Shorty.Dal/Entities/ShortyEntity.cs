using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shorty.Dal.Entities;

public class ShortyEntity
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(2048)]
    public string Url { get; set; }

    [Required, MaxLength(64)]
    public string ShortCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiredAt { get; set; }

    public long Clicks { get; set; }

    public bool IsActive { get; set; }

    public decimal Score { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public UserEntity User { get; set; }

    public ICollection<VisitEntity> Visits { get; set; }
}