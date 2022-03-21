using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorJobAPI.Infrastructure.Strings.Services
{
    public static class StringsComparer
    {
        public static int Compare(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                if (!string.IsNullOrEmpty(target))
                {
                    return target.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(target))
            {
                if (!string.IsNullOrEmpty(source))
                {
                    return source.Length;
                }
                return 0;
            }

            Int32 cost;
            Int32[,] d = new int[source.Length + 1, target.Length + 1];
            Int32 min1;
            Int32 min2;
            Int32 min3;

            for (Int32 i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (Int32 i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (Int32 i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (Int32 j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = Convert.ToInt32(!(source[i - 1] == target[j - 1]));

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }
    }
}
