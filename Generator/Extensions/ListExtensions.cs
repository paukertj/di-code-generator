using System;
using System.Collections.Generic;
using System.Text;

namespace DiDemo.Generator.Generator.Extensions
{
    internal static class ListExtensions
    {
        public static void AddIfNotEmpty<TValue>(this IList<TValue> lst, TValue value)
        {
            if (lst == null || value == null || value.Equals(default))
            {
                return;
            }

            lst.Add(value);
        }
    }
}
