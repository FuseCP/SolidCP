using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.Virtualization
{
    public class VNCConsole
    {
        public string Url {  get; set; }
        public string PVEAuthCookie { get; set; }
        public string CSRFPreventionToken { get; set; }
    }
}
