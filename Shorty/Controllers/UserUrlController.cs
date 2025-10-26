using Microsoft.AspNetCore.Mvc;
using Shorty.Dal;
using Shorty.Dal.Models;
using Shorty.Models;

namespace Shorty.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserUrlController: ControllerBase
{
    readonly UserUrlRepository _urlRepository;
  
    public UserUrlController(UserUrlRepository urlRepository)
    {
        _urlRepository = urlRepository;
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
        await _urlRepository.AddUserAsync(user);
        return Ok();

    }


    [HttpGet("get-users")]
    public async Task<IActionResult> GetAllAsinc()
    {
     
        await _urlRepository.GetAllUsersAsync();
        return Ok();

    }

}
