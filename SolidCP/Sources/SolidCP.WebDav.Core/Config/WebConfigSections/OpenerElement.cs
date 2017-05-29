using System;
using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    public class OpenerElement : ConfigurationElement
    {
        private const string ExtensionKey = "extension";
        private const string MemeTypeKey = "mimeType";
        private const string TargetBlankKey = "isTargetBlank";

        [ConfigurationProperty(ExtensionKey, IsKey = true, IsRequired = true)]
        public string Extension
        {
            get { return this[ExtensionKey].ToString(); }
            set { this[ExtensionKey] = value; }
        }

        [ConfigurationProperty(MemeTypeKey, IsKey = true, IsRequired = true)]
        public string MimeType
        {
            get { return this[MemeTypeKey].ToString(); }
            set { this[MemeTypeKey] = value; }
        }

        [ConfigurationProperty(TargetBlankKey, IsKey = true, IsRequired = true)]
        public bool IstargetBlank
        {
            get { return Convert.ToBoolean(this[TargetBlankKey]); }
            set { this[TargetBlankKey] = value; }
        }
    }
}