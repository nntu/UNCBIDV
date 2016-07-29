using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UyNhiemChiBIDV
{
    public static class LibExtensions
    {
        public static IEnumerable<T> AsNotNull<T>(this IEnumerable<T> original)
        {
            return original ?? new T[0];
        }
    }
}
