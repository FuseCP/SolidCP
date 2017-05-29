namespace SolidCP.WebDav.Core
{
    namespace Client
    {
        public class Property
        {
            public readonly PropertyName Name;
            private string _value = "";

            public Property(PropertyName name, string value)
            {
                Name = name;
                StringValue = value;
            }

            public Property(string name, string nameSpace, string value)
            {
                Name = new PropertyName(name, nameSpace);
                StringValue = value;
            }

            public string StringValue
            {
                get { return _value; }
                set { _value = value; }
            }

            public override string ToString()
            {
                return StringValue;
            }
        }
    }
}