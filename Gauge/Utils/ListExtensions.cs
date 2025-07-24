using System;
using System.Collections.Generic;

namespace Gauge.Utils
{
    public static class ListExtensions
    {
        public static bool TryFind<T>(this List<T> list, Predicate<T> match, out T value)
        {
            value = list.Find(match);

            if (value == null)
            {
                return false;
            }

            return true;
        }
    }
}
