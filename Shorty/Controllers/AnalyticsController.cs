using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
{
    [HttpGet("top/{email}")]
    public async Task<IActionResult> GetTopLinks(string email)
    {
        var links = await analyticsService.GetTopShortLinksAsync(email);

        if (links == null || links.Count == 0)
        {
            return NotFound("No links found for the specified email.");
        }

        return Ok(links);
    }

    [HttpGet("user/{email}")]
    public async Task<IActionResult> GetUserAnalytics(string email)
    {
        var analytics = await analyticsService.GetUserAnalyticsAsync(email);
        if (analytics == null)
        {
            return NotFound("User not found.");
        }
        return Ok(analytics);
    }
}