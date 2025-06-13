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

        public DvdDriveInfo Get(PSObject vmObj)
        {
            DvdDriveInfo info = null;

            PSObject result = GetPS(vmObj);

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

        public PSObject GetPS(PSObject vmObj)
        {
            Command cmd = new Command("Get-VMDvdDrive");

            cmd.Parameters.Add("VM", vmObj);

            Collection<PSObject> result = _powerShell.Execute(cmd, false); //False, because all remote connection information is already contained in vmObj

            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            
            return null;
        }

        public void Set(PSObject vmObj, string path)
        {
            var dvd = Get(vmObj);
 
            Command cmd = new Command("Set-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmObj.GetString("Name"));
            cmd.Parameters.Add("Path", path);
            cmd.Parameters.Add("ControllerNumber", dvd.ControllerNumber);
            cmd.Parameters.Add("ControllerLocation", dvd.ControllerLocation);

            _powerShell.Execute(cmd, true);
        }

        public void Update(VirtualMachine vm, PSObject vmObj, bool dvdDriveShouldBeInstalled)
        {
            if (!vm.DvdDriveInstalled && dvdDriveShouldBeInstalled)
                Add(vmObj);
            else if (vm.DvdDriveInstalled && !dvdDriveShouldBeInstalled)
                Remove(vmObj);
        }

        public void Add(PSObject vmObj)
        {
            Command cmd = new Command("Add-VMDvdDrive");

            cmd.Parameters.Add("VM", vmObj);

            _powerShell.Execute(cmd, false, true); //False, because all remote connection information is already contained in vmObj
        }

        public void Remove(PSObject vmObj)
        {
            var dvd = Get(vmObj);

            Command cmd = new Command("Remove-VMDvdDrive");

            cmd.Parameters.Add("VMName", vmObj.GetString("Name"));
            cmd.Parameters.Add("ControllerNumber", dvd.ControllerNumber);
            cmd.Parameters.Add("ControllerLocation", dvd.ControllerLocation);

            _powerShell.Execute(cmd, true, true);
        }
    }
}
