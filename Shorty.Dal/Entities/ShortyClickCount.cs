using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Dal.Entities
{
	public class ShortyClickCount 
	{
		[Key]
		public int ShortyId { get; set; }

		[Required]
		public long ClickCount { get; set; } 
	}
}

