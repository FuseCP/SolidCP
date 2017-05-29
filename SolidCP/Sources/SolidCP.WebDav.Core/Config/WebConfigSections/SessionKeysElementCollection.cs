using System.Configuration;
using SolidCP.WebDavPortal.WebConfigSections;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    [ConfigurationCollection(typeof (SessionKeysElement))]
    public class SessionKeysElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SessionKeysElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SessionKeysElement) element).Key;
        }
    }
}