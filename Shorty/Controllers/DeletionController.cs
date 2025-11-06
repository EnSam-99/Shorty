using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services;
using Shorty.Domain.Abstraction;

namespace Shorty.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeletionController(IDeletionUrlService deletionUrlService) : ControllerBase
{
    private readonly IDeletionUrlService _deletionUrlService = deletionUrlService;
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var res = await _deletionUrlService.DeleteByIdAsync(id);
        if (!res)
        {
            return NotFound($"URL with ID {id} not found.");
        }

        return Ok("URL deleted successfully.");
    }
    
    
    [HttpDelete("email/{email}")]
    public async Task<IActionResult> DeleteByEmail(string email)
    {
        var result = await _deletionUrlService.DeleteByEmailAsync(email);
        if (!result)
            return NotFound($"No URLs found for email: {email}");

        return Ok($"All URLs for {email} deleted successfully.");
    }
    
    [HttpPatch("deactivate/{id}")]
    public async Task<IActionResult> DeactivateById(int id)
    {
        var result = await _deletionUrlService.DeactivateByIdAsync(id);
        if (!result)
            return BadRequest($"Cannot deactivate URL with ID {id}. It may not exist or already deactivated.");

        return Ok("URL deactivated successfully.");
    }
    
}