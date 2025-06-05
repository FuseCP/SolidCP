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

        public DvdDriveInfo Get(string vmName)
        {
            DvdDriveInfo info = null;

            PSObject result = GetPS(vmName);

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

        public PSObject GetPS(string vmName)
        {
            Command cmd = new Command("Get-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmName);

            Collection<PSObject> result = _powerShell.Execute(cmd, true);

            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            
            return null;
        }

        public void Set(string vmName, string path)
        {
            var dvd = Get(vmName);
 
            Command cmd = new Command("Set-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmName);
            cmd.Parameters.Add("Path", path);
            cmd.Parameters.Add("ControllerNumber", dvd.ControllerNumber);
            cmd.Parameters.Add("ControllerLocation", dvd.ControllerLocation);

            _powerShell.Execute(cmd, true);
        }

        public void Update(VirtualMachine vm, bool dvdDriveShouldBeInstalled)
        {
            if (!vm.DvdDriveInstalled && dvdDriveShouldBeInstalled)
                Add(vm.Name);
            else if (vm.DvdDriveInstalled && !dvdDriveShouldBeInstalled)
                Remove(vm.Name);
        }

        public void Add(string vmName)
        {
            Command cmd = new Command("Add-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmName);

            _powerShell.Execute(cmd, true, true);
        }

        public void Remove(string vmName)
        {
            var dvd = Get(vmName);

            Command cmd = new Command("Remove-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmName);
            cmd.Parameters.Add("ControllerNumber", dvd.ControllerNumber);
            cmd.Parameters.Add("ControllerLocation", dvd.ControllerLocation);

            _powerShell.Execute(cmd, true, true);
        }
    }
}
