using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shorty.Dal.Data;
using Shorty.Dal.Entities;
using Shorty.Domain.Helpers;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto register)
    {
        if (string.IsNullOrEmpty(register.Username) || string.IsNullOrEmpty(register.Password) || string.IsNullOrEmpty(register.Email))
            return BadRequest("All fields are required.");

        if (await _db.Users.AnyAsync(u => u.Username == register.Username))
            return BadRequest("Username already exists.");

        if (await _db.Users.AnyAsync(u => u.Email == register.Email))
            return BadRequest("Email already registered.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = register.Username,
            Email = register.Email,
            PasswordHash = PasswordHelper.HashPassword(register.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var hash = PasswordHelper.HashPassword(login.Password);
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == login.Username && u.PasswordHash == hash);

        if (user == null) return Unauthorized("Invalid username or password.");

        var secret = _config.GetValue<string>("JwtSecret");
        var token = JwtHelper.GenerateToken(user.Id, user.Username, secret);

        return Ok(new { Token = token });
    }
}
