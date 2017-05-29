namespace SolidCP.WebDav.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceLast(this string source, string target, string newValue)
        {
            int index = source.LastIndexOf(target);
            string result = source.Remove(index, target.Length).Insert(index, newValue);
            return result;
        }

        public static string Tail(this string source, int tailLength)
        {
            if (source == null || tailLength >= source.Length)
            {
                return source;
            }

            return source.Substring(source.Length - tailLength);
        }

        public static string RemoveLeadingFromPath(this string source, string toRemove)
        {
            return source.StartsWith('/' + toRemove) ? source.Substring(toRemove.Length + 1) : source;
        }
    }
}