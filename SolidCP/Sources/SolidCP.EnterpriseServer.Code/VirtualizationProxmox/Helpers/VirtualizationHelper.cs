using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolidCP.Providers.VirtualizationProxmox;

namespace SolidCP.EnterpriseServer.Code.VirtualizationProxmox
{
    public static class VirtualizationHelper
    {
        public static VirtualizationServerProxmox GetVirtualizationProxy(int serviceId)
        {
            VirtualizationServerProxmox ws = new VirtualizationServerProxmox();
            ServiceProviderProxy.Init(ws, serviceId);
            return ws;
        }
    }
}
