using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    public class FilesToIgnoreElement : ConfigurationElement
    {
        private const string NameKey = "name";
        private const string RegexKey = "regex";

        [ConfigurationProperty(NameKey, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this[NameKey].ToString(); }
            set { this[NameKey] = value; }
        }

        [ConfigurationProperty(RegexKey, IsKey = true, IsRequired = true)]
        public string Regex
        {
            get { return this[RegexKey].ToString(); }
            set { this[RegexKey] = value; }
        }
    }
}