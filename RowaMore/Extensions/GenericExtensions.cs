using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RowaMore.Extensions
{
    public static class GenericExtensions
    {
        public static T Pipe<T>(this T thisValue, Action<T> action)
        {
            action?.Invoke(thisValue);
            return thisValue;
        }

        public static T Pipe<T>(this T thisValue, Func<T, T> func)
            => func.Invoke(thisValue);

        public static bool In<T>(this T thisValue, params T[] @params)
            => thisValue == null ? false : @params.Contains(thisValue);
    }
}
