namespace SolidCP.WebDav.Core
{
    namespace Client
    {
        public sealed class PropertyName
        {
            public readonly string Name;
            public readonly string NamespaceUri;

            public PropertyName(string name, string namespaceUri)
            {
                Name = name;
                NamespaceUri = namespaceUri;
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() == GetType())
                {
                    if (((PropertyName) obj).Name == Name && ((PropertyName) obj).NamespaceUri == NamespaceUri)
                    {
                        return true;
                    }
                }

                return false;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}