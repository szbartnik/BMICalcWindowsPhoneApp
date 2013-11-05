using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp1.Infrastructure
{
    public static class Extensions
    {
        public static bool Between<T>(this T value, T min, T max, bool inclusive = false)
        {
            return inclusive
                ? (Comparer<T>.Default.Compare(value, min) >= 0
                    && Comparer<T>.Default.Compare(value, max) <= 0)
                : (Comparer<T>.Default.Compare(value, min) > 0
                    && Comparer<T>.Default.Compare(value, max) < 0);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }
    }
}
