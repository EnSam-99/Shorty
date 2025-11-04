using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Domain.Models.Response
{
	public class TopShortyDto
	{
		public int Id { get; set; }
		public string ShortyUrl { get; set; } = string.Empty;
		public string OriginalUrl { get; set; } = string.Empty;
		public int ClickCount { get; set; }
	}
}
