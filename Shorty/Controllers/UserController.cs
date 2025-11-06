using Microsoft.AspNetCore.Mvc;
using Shorty.Dal.Models;
using Shorty.Models;
using Shorty.Services.IServices;

namespace Shorty.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService<UserEntity> _userService) : ControllerBase
{
    [HttpPost("create-user")]
    public async Task<IActionResult> AddUserAsync([FromBody] UserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
            return BadRequest("Name and Email are required.");

        var user = await _userService.CreateUserAsync(userName: dto.Name, email: dto.Email);
        return Ok(new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
    }
}





