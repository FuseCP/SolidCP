using System;
using System.Configuration;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    [ConfigurationCollection(typeof(OpenerElement))]
    public class OpenerElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new OpenerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((OpenerElement)element).Extension;
        }
    }
}