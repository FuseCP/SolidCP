using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.Virtualization2012;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012
{
    public static class ReplicationHelper
    {

        public static void CleanUpReplicaServer(VirtualMachine originalVm)
        {
            try
            {
                ResultObject result = new ResultObject();

                // Get replica server
                var replicaServer = GetReplicaForService(originalVm.ServiceId, ref result);

                // Clean up replica server
                var replicaVm = replicaServer.GetVirtualMachines().FirstOrDefault(m => m.Name == originalVm.Name);
                if (replicaVm != null)
                {
                    replicaServer.DisableVmReplication(replicaVm.VirtualMachineId);
                    replicaServer.ShutDownVirtualMachine(replicaVm.VirtualMachineId, true, "ReplicaDelete");
                    replicaServer.DeleteVirtualMachine(replicaVm.VirtualMachineId);
                }
            }
            catch { /* skip */ }
        }

        public static ReplicationServerInfo GetReplicaInfoForService(int serviceId, ref ResultObject result)
        {
            // Get service id of replica server
            StringDictionary vsSesstings = ServerController.GetServiceSettings(serviceId);
            string replicaServiceId = vsSesstings["ReplicaServerId"];

            if (string.IsNullOrEmpty(replicaServiceId))
            {
                result.ErrorCodes.Add(VirtualizationErrorCodes.NO_REPLICA_SERVER_ERROR);
                return null;
            }

            // get replica server info for replica service id
            VirtualizationServer2012 vsReplica = VirtualizationHelper.GetVirtualizationProxy(Convert.ToInt32(replicaServiceId));
            StringDictionary vsReplicaSesstings = ServerController.GetServiceSettings(Convert.ToInt32(replicaServiceId));
            string computerName = vsReplicaSesstings["ServerName"];
            var replicaServerInfo = vsReplica.GetReplicaServer(computerName);

            if (!replicaServerInfo.Enabled)
            {
                result.ErrorCodes.Add(VirtualizationErrorCodes.NO_REPLICA_SERVER_ERROR);
                return null;
            }

            return replicaServerInfo;
        }

        public static VirtualizationServer2012 GetReplicaForService(int serviceId, ref ResultObject result)
        {
            // Get service id of replica server
            StringDictionary vsSesstings = ServerController.GetServiceSettings(serviceId);
            string replicaServiceId = vsSesstings["ReplicaServerId"];

            if (string.IsNullOrEmpty(replicaServiceId))
            {
                result.ErrorCodes.Add(VirtualizationErrorCodes.NO_REPLICA_SERVER_ERROR);
                return null;
            }

            // get replica server for replica service id
            return VirtualizationHelper.GetVirtualizationProxy(Convert.ToInt32(replicaServiceId));
        }

        public static void CheckReplicationQuota(int packageId, ref ResultObject result)
        {
            List<string> quotaResults = new List<string>();
            PackageContext cntx = PackageController.GetPackageContext(packageId);

            QuotaHelper.CheckBooleanQuota(cntx, quotaResults, Quotas.VPS2012_REPLICATION_ENABLED, true, VirtualizationErrorCodes.QUOTA_REPLICATION_ENABLED);

            if (quotaResults.Count > 0)
                result.ErrorCodes.AddRange(quotaResults);
        }
    }
}
