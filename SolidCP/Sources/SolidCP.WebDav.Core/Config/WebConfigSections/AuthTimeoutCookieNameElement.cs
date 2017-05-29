using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.WebDav.Core.Config.WebConfigSections
{
    public class AuthTimeoutCookieNameElement : ConfigurationElement
    {
        private const string ValueKey = "value";

        [ConfigurationProperty(ValueKey, IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)this[ValueKey]; }
            set { this[ValueKey] = value; }
        }
    }
}
