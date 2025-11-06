using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Entities
{
    public class ShortLink
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } = string.Empty;

        [Required]
        public string ShortyUrl { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User? User { get; set; }
        
        public bool IsActive { get; set; } = false;

        public decimal Score { get; set; } = 0m;

        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}
