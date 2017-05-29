using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.RemoteDesktopServices
{    
    [Serializable]
    public class RdsUserSession
    {
        public string CollectionName { get; set; }
        public string UserName { get; set; }
        public string UnifiedSessionId { get; set; }
        public string SessionState { get; set; }
        public string HostServer { get; set; }
        public string DomainName { get; set; }
        public bool IsVip { get; set; }
        public string SamAccountName { get; set; }
    }
}
