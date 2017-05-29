using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolidCP.Providers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LogPropertyAttribute : Attribute
    {
        public LogPropertyAttribute()
        {
        }

        public LogPropertyAttribute(string nameInLog)
        {
            NameInLog = nameInLog;
        }

        public string NameInLog { get; set; }

        public string GetLogString(object obj, PropertyInfo propertyInfo)
        {
            if (obj != null && propertyInfo != null)
            {
                var value = LogExtensionHelper.GetString(propertyInfo.GetValue(obj, null));
                return GetLogString(propertyInfo.Name, value);
            }

            return "";
        }

        public string GetLogString(string name, string value)
        {
            var logName = string.IsNullOrEmpty(NameInLog) ? LogExtensionHelper.DecorateName(name) : NameInLog;
            return LogExtensionHelper.CombineString(logName, value);
        }
    }
}
