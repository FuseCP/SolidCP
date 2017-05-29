using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LogParentPropertyAttribute : LogPropertyAttribute
    {
        public LogParentPropertyAttribute(string name) : base(null)
        {
            Name = name;
        }

        public string Name { get; set; }

        public string GetLogString<T>(T obj)
        {
            var property = typeof (T).GetProperty(Name);
            return GetLogString(obj, property);
        }
    }
}
