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
    public class DvdDriveHelper
    {
        private PowerShellManager _powerShell;
        private MiManager _mi;

        public DvdDriveHelper(PowerShellManager powerShellManager, MiManager mi)
        {
            _powerShell = powerShellManager;
            _mi = mi;
        }

        public DvdDriveInfo Get(VirtualMachineData vmData)
        {
            DvdDriveInfo info = null;

            PSObject result = GetPS(vmData);

            if (result != null)
            {
                info = new DvdDriveInfo();
                info.Id = result.GetString("Id");
                info.Name = result.GetString("Name");
                info.ControllerType = result.GetEnum<ControllerType>("ControllerType");
                info.ControllerNumber = result.GetInt("ControllerNumber");
                info.ControllerLocation = result.GetInt("ControllerLocation");
                info.Path = result.GetString("Path");
            }
            return info;
        }

        public PSObject GetPS(VirtualMachineData vmData)
        {
            Command cmd = new Command("Get-VMDvdDrive");

            Collection<PSObject> result = _powerShell.ExecuteOnVm(cmd, vmData);

            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            
            return null;
        }

        public void Set(VirtualMachineData vmData, string path)
        {
            var dvd = Get(vmData);
 
            Command cmd = new Command("Set-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmData.VM.Name);
            cmd.Parameters.Add("Path", path);
            cmd.Parameters.Add("ControllerNumber", dvd.ControllerNumber);
            cmd.Parameters.Add("ControllerLocation", dvd.ControllerLocation);

            _powerShell.Execute(cmd, true);
        }

        public void Update(VirtualMachineData vmData, bool dvdDriveShouldBeInstalled)
        {
            if (!vmData.VM.DvdDriveInstalled && dvdDriveShouldBeInstalled)
                Add(vmData);
            else if (vmData.VM.DvdDriveInstalled && !dvdDriveShouldBeInstalled)
                Remove(vmData);
        }

        public void Add(VirtualMachineData vmData)
        {
            Command cmd = new Command("Add-VMDvdDrive");

            _powerShell.ExecuteOnVm(cmd, vmData, true);
        }

        public void Remove(VirtualMachineData vmData)
        {
            var dvd = Get(vmData);

            Command cmd = new Command("Remove-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmData.VM.Name);
            cmd.Parameters.Add("ControllerNumber", dvd.ControllerNumber);
            cmd.Parameters.Add("ControllerLocation", dvd.ControllerLocation);

            _powerShell.Execute(cmd, true, true);
        }
    }
}
