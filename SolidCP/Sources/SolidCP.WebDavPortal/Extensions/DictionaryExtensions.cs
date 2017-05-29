using System;
using System.Collections.Generic;

namespace SolidCP.WebDavPortal.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (source == null)
                throw new ArgumentNullException("source");
            foreach (var element in source)
                target.Add(element);
        }
    }
}