using System;
using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    [ConfigurationCollection(typeof(FilesToIgnoreElement))]
    public class FilesToIgnoreElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FilesToIgnoreElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FilesToIgnoreElement)element).Name;
        }
    }
}