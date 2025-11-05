using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Models.Response
{
    public class TopShortLinkDto
    {
        public string ShortUrl { get; set; } = "";
        public string OriginalUrl { get; set; } = "";
        public long Clicks { get; set; }
    }
}
