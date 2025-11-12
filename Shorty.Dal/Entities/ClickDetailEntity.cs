using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shorty.Dal.Entities
{
    public class ClickDetailEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ShortyId { get; set; }

        [ForeignKey(nameof(ShortyId))]
        public ShortyEntity Shorty { get; set; }

        [Required, MaxLength(64)]
        public string IpAddress { get; set; }

        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;
    }
}