using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    public class UserDomainElement : ConfigurationElement
    {
        private const string ValueKey = "value";

        [ConfigurationProperty(ValueKey, IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string) this[ValueKey]; }
            set { this[ValueKey] = value; }
        }
    }
}