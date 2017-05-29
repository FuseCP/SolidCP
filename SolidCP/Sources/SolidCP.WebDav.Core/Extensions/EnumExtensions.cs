using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using SolidCP.WebDav.Core.Attributes.Resources;

namespace SolidCP.WebDav.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}