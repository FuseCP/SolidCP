using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.RemoteDesktopServices
{
    public class RdsMessageRecipient
    {        
        public string SessionId { get; set; }

        public string ComputerName { get; set; }
    }
}
