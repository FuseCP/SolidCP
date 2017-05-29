using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.VirtualizationProxmox;

namespace SolidCP.EnterpriseServer.Code.VirtualizationProxmox
{
    public static class ReplicationHelper
    {

        public static void CleanUpReplicaServer(VirtualMachine originalVm)
        {
            throw new NotImplementedException();
        }

        public static ReplicationServerInfo GetReplicaInfoForService(int serviceId, ref ResultObject result)
        {
            throw new NotImplementedException();
        }

        public static VirtualizationServerProxmox GetReplicaForService(int serviceId, ref ResultObject result)
        {
            throw new NotImplementedException();
        }

        public static void CheckReplicationQuota(int packageId, ref ResultObject result)
        {
            throw new NotImplementedException();
        }
    }
}
