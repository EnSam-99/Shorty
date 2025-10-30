namespace Shorty.Models;

public class UserDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; } =  default!;
    public string? Email { get; set; } =  default!;
}

