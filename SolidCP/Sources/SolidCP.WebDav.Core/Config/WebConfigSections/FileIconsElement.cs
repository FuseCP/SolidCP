using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    public class FileIconsElement : ConfigurationElement
    {
        private const string ExtensionKey = "extension";
        private const string PathKey = "path";

        [ConfigurationProperty(ExtensionKey, IsKey = true, IsRequired = true)]
        public string Extension
        {
            get { return (string) this[ExtensionKey]; }
            set { this[ExtensionKey] = value; }
        }

        [ConfigurationProperty(PathKey, IsKey = true, IsRequired = true)]
        public string Path
        {
            get { return (string) this[PathKey]; }
            set { this[PathKey] = value; }
        }
    }
}