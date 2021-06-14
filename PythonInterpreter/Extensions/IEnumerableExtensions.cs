using System;
using System.Collections.Generic;
using System.Linq;

namespace PythonInterpreter
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            source.ToList().ForEach(action);
        }
    }
}
