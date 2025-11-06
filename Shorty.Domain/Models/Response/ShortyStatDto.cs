using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Models.Response
{
    public class ShortyStatDto
    {
        public string Url { get; set; }
        public int Clicks { get; set; }
        public DateTime? LastAccessedDate { get; set; }
    }
}
