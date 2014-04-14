using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdUtils
{
    public static class StringUtils
    {
        public static string Enquote(string input)
        {
            return String.Format("{0}{1}{0}", '"', input);
        }
    }
}
