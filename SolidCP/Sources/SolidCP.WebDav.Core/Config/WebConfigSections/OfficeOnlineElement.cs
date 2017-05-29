using System.Configuration;

namespace SolidCP.WebDavPortal.WebConfigSections
{
    public class OfficeOnlineElement : ConfigurationElement
    {
        private const string ExtensionKey = "extension";
        private const string OwaViewKey = "OwaView";
        private const string OwaEditorKey = "OwaEditor";
        private const string OwaMobileViewKey = "OwaMobileView";
        private const string OwaNewFileViewKey = "OwaNewFileView";

        [ConfigurationProperty(ExtensionKey, IsKey = true, IsRequired = true)]
        public string Extension
        {
            get { return this[ExtensionKey].ToString(); }
            set { this[ExtensionKey] = value; }
        }

        [ConfigurationProperty(OwaViewKey, IsKey = true, IsRequired = true)]
        public string OwaView
        {
            get { return this[OwaViewKey].ToString(); }
            set { this[OwaViewKey] = value; }
        }

        [ConfigurationProperty(OwaEditorKey, IsKey = true, IsRequired = true)]
        public string OwaEditor
        {
            get { return this[OwaEditorKey].ToString(); }
            set { this[OwaEditorKey] = value; }
        }


        [ConfigurationProperty(OwaMobileViewKey, IsKey = true, IsRequired = true)]
        public string OwaMobileViev
        {
            get { return this[OwaMobileViewKey].ToString(); }
            set { this[OwaMobileViewKey] = value; }
        }

        [ConfigurationProperty(OwaNewFileViewKey, IsKey = true, IsRequired = true)]
        public string OwaNewFileView
        {
            get { return this[OwaNewFileViewKey].ToString(); }
            set { this[OwaNewFileViewKey] = value; }
        }
    }
}