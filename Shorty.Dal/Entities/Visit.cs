using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Entities
{
    public class Visit
    {
        public int Id { get; set; }
        public int ShortLinkId { get; set; }

        public ShortLink ShortLink { get; set; } = null!;
        public DateTime VisitDate { get; set; }
    }
}
