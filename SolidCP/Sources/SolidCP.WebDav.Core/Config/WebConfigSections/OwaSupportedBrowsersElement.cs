using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    public class OwaSupportedBrowsersElement : ConfigurationElement
    {
        private const string BrowserKey = "browser";
        private const string VersionKey = "version";

        [ConfigurationProperty(BrowserKey, IsKey = true, IsRequired = true)]
        public string Browser
        {
            get { return (string)this[BrowserKey]; }
            set { this[BrowserKey] = value; }
        }

        [ConfigurationProperty(VersionKey, IsKey = true, IsRequired = true)]
        public int Version
        {
            get { return (int)this[VersionKey]; }
            set { this[VersionKey] = value; }
        }
    }
}