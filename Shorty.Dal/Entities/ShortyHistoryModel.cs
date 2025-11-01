using Shorty.Dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Entities;

public class ShortyHistoryModel
{
    [Key] public Guid Id { get; set; }    
    [MaxLength(255)] public string? OldShortUrl { get; set; } 
    [Required, MaxLength(255)] public string NewShortUrl { get; set; } = null!;
    [Required] public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [Required] public Guid ShortyId { get; set; }
    [ForeignKey(nameof(ShortyId))] public ShortyModel Shorty { get; set; } = null!;
}
