namespace Shorty.Dal;

public class AdminUsersDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsAdmin { get; set; }
    public int LinksCount { get; set; }
    public long TotalClicks { get; set; }
    public int ActiveLinks { get; set; }
}
