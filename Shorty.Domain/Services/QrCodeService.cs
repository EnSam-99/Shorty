using QRCoder;
using Shorty.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Services
{
    public class QrCodeService : IQrCodeService
    {
        private readonly string _qrFolderPath;

        public QrCodeService()
        {
            _qrFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes");

            if (!Directory.Exists(_qrFolderPath))
                Directory.CreateDirectory(_qrFolderPath);
        }
        public async Task<string> GenerateQrCodeAsync(string content, int shortyId)
        {
            var fileName = $"shorty_{shortyId}.jpg";
            var filePath = Path.Combine(_qrFolderPath, fileName);

            await Task.Run(() =>
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new QRCode(qrCodeData);
                using var bitmap = qrCode.GetGraphic(20);
                bitmap.Save(filePath, ImageFormat.Jpeg);
            });

            return $"/qrcodes/{fileName}";
        }
    }
}
