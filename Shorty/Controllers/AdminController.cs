using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IAdminService _adminService): ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsersForAdminAsync()
    {
        try
        {
            var users = await _adminService.GetAllUsersForAdminAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {           
            Console.WriteLine(ex);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteUserAsync(int userId)
    {
        await _adminService.DeleteUserAsync(userId);
        return Ok();
    }

    [HttpDelete("delete-inactive-shorties")]
    public async Task<IActionResult> DeleteAllInactiveLinks()
    {
        var deletedCount = await _adminService.DeleteAllInactiveLinks();
        return Ok(new { DeletedCount = deletedCount });
    }

    [HttpPatch("deactivate-expired-shorties")]
    public async Task<IActionResult> DeactivateExpiredShortiesAsync()
    {
        var deactivatedCount = await _adminService.DeactivateExpiredShortiesAsync();
        return Ok(new { DeactivatedCount = deactivatedCount });
    }
}
