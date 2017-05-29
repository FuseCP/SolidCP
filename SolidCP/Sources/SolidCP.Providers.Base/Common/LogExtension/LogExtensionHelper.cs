using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SolidCP.Providers
{
    public class LogExtensionHelper
    {
        public const string LOG_STRING_TEMPLATE = "{0}: {1}";
        public const string LOG_ARRAY_SEPARATOR = ", ";

        public static string CombineString(string name, string value)
        {
            if (name == null)
                name = "";

            if (value == null)
                value = "";

            return String.Format(LOG_STRING_TEMPLATE, name, value);
        }

        public static string DecorateName(string name)
        {
            if (name == null)
                return "";

            name = Regex.Replace(name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0"); // "DriveIsSCSICompatible" becomes "Drive Is SCSI Compatible"
            name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name); // Capitalize
            name = Regex.Replace(name, @"\bId\b", "ID", RegexOptions.IgnoreCase); // "Id" becomes "ID"

            return name;
        }

        public static string GetString(object value)
        {
            if (value == null)
                return "";

            // if array
            if (value.GetType().IsArray)
            {
                var elementType = value.GetType().GetElementType();

                if (elementType != null && !elementType.IsValueType)
                {
                    string[] strs = ((IEnumerable) value).Cast<object>().Select(x => x.ToString()).ToArray();
                    return string.Join(LOG_ARRAY_SEPARATOR, strs);
                }
            }

            return value.ToString();
        }
    }
}
