using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.guacamole
{
    public class guacadata
    {
        public string protocol { get; set; }
        public string hostname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string domain { get; set; }
        public string port { get; set; }
        public string security { get; set; }
        public string preconnectionblob { get; set; }
        public string vmhostname { get; set; }
    }
}