using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    [ConfigurationCollection(typeof (FileIconsElement))]
    public class FileIconsElementCollection : ConfigurationElementCollection
    {
        private const string DefaultPathKey = "defaultPath";
        private const string FolderPathKey = "folderPath";

        [ConfigurationProperty(DefaultPathKey, IsRequired = false, DefaultValue = "/")]
        public string DefaultPath
        {
            get { return (string) this[DefaultPathKey]; }
            set { this[DefaultPathKey] = value; }
        }

        [ConfigurationProperty(FolderPathKey, IsRequired = false)]
        public string FolderPath
        {
            get { return (string)this[FolderPathKey]; }
            set { this[FolderPathKey] = value; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FileIconsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileIconsElement) element).Extension;
        }
    }
}