using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shorty.Dal.Entities
{
    public class UrlMapping
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string OriginalURL { get; set; } = string.Empty;
        [Required]
        public string ShortURL { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

    }
}

