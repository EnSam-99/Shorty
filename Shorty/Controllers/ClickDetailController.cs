using Microsoft.AspNetCore.Mvc;
using Shorty.Domain.Services.Abstractions;
using Shorty.Domain.Models;

namespace Shorty.Controllers
{
    [ApiController]
    [Route("shorties")]
    public class ClickDetailController(IClickDetailService _clickDetailService) : ControllerBase
    {
        
        [HttpGet("{id}/click-details")]
        public async Task<ActionResult<ClickDetailDto>> GetClickDetails(int id)
        {
            var summary = await _clickDetailService.GetClickSummaryAsync(id);

            return Ok(new
            {
                ipaddress = summary.IpAddress,
                shortyCount = summary.ShortyCount,
                VisitCount = summary.VisitCount
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> AddClick(int shortyId)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            await _clickDetailService.AddClickAsync(shortyId, ip);
            return Ok(new { Message = "Click tracked successfully", Ip = ip });
        }
    }
}