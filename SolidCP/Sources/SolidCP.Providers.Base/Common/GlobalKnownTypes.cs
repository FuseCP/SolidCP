using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Reflection;

namespace SolidCP.Providers
{
    public class GlobalKnownTypes
    {
        public static ImmutableHashSet<Type> KnownTypes = ImmutableHashSet<Type>.Empty;

        public static bool AddKnownType(Type type)
        {
            return ImmutableInterlocked.Update(ref KnownTypes, knownType => knownType.Add(type));
        }
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider) => KnownTypes;
    }
}
