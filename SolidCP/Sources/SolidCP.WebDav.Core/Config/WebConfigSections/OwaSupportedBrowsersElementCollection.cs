using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    [ConfigurationCollection(typeof(OwaSupportedBrowsersElement))]
    public class OwaSupportedBrowsersElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new OwaSupportedBrowsersElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((OwaSupportedBrowsersElement)element).Browser;
        }
    }
}