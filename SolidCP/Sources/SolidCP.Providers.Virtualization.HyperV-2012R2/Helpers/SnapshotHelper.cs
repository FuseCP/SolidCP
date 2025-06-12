using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    public static class SnapshotHelper
    {
        public static VirtualMachineSnapshot GetFromPS(PSObject psObject, string runningSnapshotId = null)
        {
            var snapshot = new VirtualMachineSnapshot
            {
                Id = psObject.GetString("Id"),
                Name = psObject.GetString("Name"),
                VMName = psObject.GetString("VMName"),
                ParentId = psObject.GetString("ParentSnapshotId"),
                Created = psObject.GetProperty<DateTime>("CreationTime"),
                SnapshotType = psObject.GetString("SnapshotType")
            };

            if (string.IsNullOrEmpty(snapshot.ParentId))
                snapshot.ParentId = null; // for capability

            if (!String.IsNullOrEmpty(runningSnapshotId))
                snapshot.IsCurrent = snapshot.Id == runningSnapshotId;

            return snapshot;
        }

        public static VirtualMachineSnapshot GetFromCim(CimInstance objSnapshot)
        {
            if (objSnapshot == null || objSnapshot.CimInstanceProperties.Count == 0)
                return null;

            VirtualMachineSnapshot snapshot = new VirtualMachineSnapshot();
            snapshot.Id = (string)objSnapshot.CimInstanceProperties["InstanceID"].Value;
            snapshot.Name = (string)objSnapshot.CimInstanceProperties["ElementName"].Value;

            string parentId = (string)objSnapshot.CimInstanceProperties["Parent"].Value;
            if (!String.IsNullOrEmpty(parentId))
            {
                int idx = parentId.IndexOf("Microsoft:");
                snapshot.ParentId = parentId.Substring(idx, parentId.Length - idx - 1);
                snapshot.ParentId = snapshot.ParentId.ToLower().Replace("microsoft:", "");
            }
            if (!String.IsNullOrEmpty(snapshot.Id))
            {
                snapshot.Id = snapshot.Id.ToLower().Replace("microsoft:", "");
            }
            snapshot.Created = (DateTime)objSnapshot.CimInstanceProperties["CreationTime"].Value;

            if (string.IsNullOrEmpty(snapshot.ParentId))
                snapshot.ParentId = null; // for capability

            return snapshot;
        }

        public static void Delete(PowerShellManager powerShell, VirtualMachineSnapshot snapshot, bool includeChilds)
        {
            Command cmd = new Command("Remove-VMSnapshot");
            cmd.Parameters.Add("VMName", snapshot.VMName);
            cmd.Parameters.Add("Name", snapshot.Name);
            if (includeChilds) cmd.Parameters.Add("IncludeAllChildSnapshots", true);

            powerShell.Execute(cmd, true);
        }

        public static void Delete(PowerShellManager powerShell, string vmName)
        {
            Command cmd = new Command("Remove-VMSnapshot");
            cmd.Parameters.Add("VMName", vmName);

            powerShell.Execute(cmd, true);
        }
    }
}
