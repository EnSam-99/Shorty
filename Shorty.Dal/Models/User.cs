using System.ComponentModel.DataAnnotations;

namespace Shorty.Dal.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Email{ get; set; } = string.Empty;
    public IList<Shorty> _shorties { get; set; } = new List<Shorty>();

}