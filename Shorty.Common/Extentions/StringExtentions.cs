using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorty.Common.Extentions
{
    public static class StringExtentions
    {
        public static string NormalizeUrl(this string url)
        {
            if (string.IsNullOrWhiteSpace(url)) 
                return url;

            url = url.ToLower().Trim();
            url = url.TrimEnd('/');
            if (url.StartsWith("www."))
                url = url.Substring(4);

            return url;
        }
    }
}
