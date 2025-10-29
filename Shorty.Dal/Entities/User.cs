using System.ComponentModel.DataAnnotations;

namespace Shorty.Dal.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Email{ get; set; } = string.Empty;
    public IList<Shorty> Shorties { get; set; } = new List<Shorty>();

}