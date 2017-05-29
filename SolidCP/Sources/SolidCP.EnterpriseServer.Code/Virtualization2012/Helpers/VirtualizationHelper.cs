using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolidCP.Providers.Virtualization2012;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012
{
    public static class VirtualizationHelper
    {
        public static VirtualizationServer2012 GetVirtualizationProxy(int serviceId)
        {
            VirtualizationServer2012 ws = new VirtualizationServer2012();
            ServiceProviderProxy.Init(ws, serviceId);
            return ws;
        }
    }
}
