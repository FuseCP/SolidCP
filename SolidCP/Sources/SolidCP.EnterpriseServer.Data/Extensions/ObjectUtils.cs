using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SolidCP.EnterpriseServer.Data
{
	public class ObjectUtils
	{
		public static bool IsAnonymous(Type type)
		{
			return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) &&
				type.IsGenericType &&
				(type.Name.Contains("AnonymousType") || type.Name.Contains("AnonType")) &&
				(type.Name.StartsWith("<>") || type.Name.StartsWith("VB$")) &&
				type.Attributes.HasFlag(TypeAttributes.NotPublic);
		}

		private static Hashtable typeProperties = new Hashtable();

		public static PropertyInfo[] GetTypeProperties(Type type)
		{
			string typeName = type.AssemblyQualifiedName;
			if (typeProperties[typeName] != null)
				return (PropertyInfo[])typeProperties[typeName];

			PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			typeProperties[typeName] = props;
			return props;
		}
	}
}
