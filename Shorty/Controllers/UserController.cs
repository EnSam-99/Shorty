using Microsoft.AspNetCore.Mvc;
using Shorty.Dal;
using Shorty.Dal.Models;
using Shorty.Models;

namespace Shorty.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController: ControllerBase
{
    readonly UserRepository _userRepository;
  
    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("create-user")]
    public async Task<IActionResult> AddUserAsinc([FromBody] UserDto dto)
    {
        if(string.IsNullOrWhiteSpace(dto.Name)|| string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest("Empty fields");
        }
        var user = new User
        {
            Name = dto.Name.Trim(),
            Email = dto.Email.Trim()
        };
        await _userRepository.AddUserAsync(user);
        return Ok();

    }


    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {

        var users = await _userRepository.GetAllUsersAsync();
        var result = users.Select(u => new UserDto()
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
        });
        return Ok(result);

    }

}
