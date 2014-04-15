using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StdUtils
{
    public class FileSizeUtils
    {
        const int scale = 1024;

        public static string FormatBytes(long bytes)
        {
            var orders = new [] { "GB", "MB", "KB", "Bytes" };
            var max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }
    }
}
