using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    public static class MemoryHelper
    {
        public static DynamicMemory GetDynamicMemory(PowerShellManager powerShell, string vmName)
        {
            DynamicMemory info = null;

            Command cmd = new Command("Get-VMMemory");
            cmd.Parameters.Add("VMName", vmName);
            Collection<PSObject> result = powerShell.Execute(cmd);

            if (result != null && result.Count > 0)
            {
                info = new DynamicMemory();
                info.Enabled = result[0].GetBool("DynamicMemoryEnabled");
                info.Minimum = Convert.ToInt32(result[0].GetLong("Minimum") / Constants.Size1M);
                info.Maximum = Convert.ToInt32(result[0].GetLong("Maximum") / Constants.Size1M);
                info.Buffer = Convert.ToInt32(result[0].GetInt("Buffer"));
                info.Priority = Convert.ToInt32(result[0].GetInt("Priority"));
            }

            return info;
        }

        public static void Update(PowerShellManager powerShell, VirtualMachine vm, int ramMb, DynamicMemory dynamicMemory)
        {
            Command cmd = new Command("Set-VMMemory");

            cmd.Parameters.Add("VMName", vm.Name);
            cmd.Parameters.Add("StartupBytes", ramMb * Constants.Size1M);

            if (dynamicMemory != null && dynamicMemory.Enabled)
            {
                cmd.Parameters.Add("DynamicMemoryEnabled", true);
                cmd.Parameters.Add("MinimumBytes", dynamicMemory.Minimum * Constants.Size1M);
                cmd.Parameters.Add("MaximumBytes", dynamicMemory.Maximum * Constants.Size1M);
                cmd.Parameters.Add("Buffer", dynamicMemory.Buffer);
                cmd.Parameters.Add("Priority", dynamicMemory.Priority);
            }
            else
            {
                cmd.Parameters.Add("DynamicMemoryEnabled", false);
            }

            powerShell.Execute(cmd, true, true);
        }
    }
}
