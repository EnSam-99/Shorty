using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Services.Abstractions
{
    public interface IQrCodeService
    {
        Task<string> GenerateQrCodeAsync(string content, int shortyId);
    }
}
