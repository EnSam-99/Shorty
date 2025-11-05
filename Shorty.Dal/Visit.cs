using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal
{

    public class Visit
    {
        public int Id { get; set; }

        public int ShortLinkId { get; set; }
        public ShortLink ShortLink { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
