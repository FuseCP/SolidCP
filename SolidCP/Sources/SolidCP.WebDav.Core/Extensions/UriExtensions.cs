using System;
using System.Linq;

namespace SolidCP.WebDav.Core.Extensions
{
    public static class UriExtensions
    {
        public static Uri Append(this Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/'))));
        }

        public static string ToStringPath(this Uri uri)
        {
            var hostStart = uri.ToString().IndexOf(uri.Host, System.StringComparison.Ordinal);
            var hostLength = uri.Host.Length;

            return uri.ToString().Substring(hostStart + hostLength, uri.ToString().Length - hostStart - hostLength);
        }

        public static string GetParentUriString(this Uri uri)
        {
            return uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length);
        }
    }
}