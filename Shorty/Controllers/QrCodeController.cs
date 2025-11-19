using Microsoft.AspNetCore.Mvc;
using Shorty.Dal;
using Shorty.Domain.Services.Abstractions;

namespace Shorty.Controllers
{
    [ApiController]
    [Route("api/shorties/{id}/qrcode")]
    public class QrCodeController : ControllerBase
    {
        private readonly IQrCodeService _qrCodeService;
        private readonly AppDbContext _context;

        public QrCodeController(IQrCodeService qrService, AppDbContext context)
        {
            _qrCodeService = qrService;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GenerateQrCode(int id)
        {
            var shorty = await _context.Shorties.FindAsync(id);
            if (shorty == null)
                return NotFound("Shorty not found.");

            var qrPath = await _qrCodeService.GenerateQrCodeAsync(shorty.ShortCode, id);
            shorty.QrPath = qrPath;
            _context.Update(shorty);
            await _context.SaveChangesAsync();

            var fileFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", qrPath.TrimStart('/'));
            var fileBytes = await System.IO.File.ReadAllBytesAsync(fileFullPath);

            return File(fileBytes, "image/jpeg");
        }
    }
}
