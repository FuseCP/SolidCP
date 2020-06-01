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
    public static class HardDriveHelper
    {
        public static VirtualHardDiskInfo[] Get(PowerShellManager powerShell, string vmname)
        {
            List<VirtualHardDiskInfo> disks = new List<VirtualHardDiskInfo>();

            Collection<PSObject> result = GetPS(powerShell, vmname);

            if (result != null && result.Count > 0)
            {
                foreach (PSObject d in result)
                {
                    VirtualHardDiskInfo disk = new VirtualHardDiskInfo();

                    disk.SupportPersistentReservations = Convert.ToBoolean(d.GetProperty("SupportPersistentReservations"));
                    disk.MaximumIOPS = Convert.ToUInt64(d.GetProperty("MaximumIOPS"));
                    disk.MinimumIOPS = Convert.ToUInt64(d.GetProperty("MinimumIOPS"));
                    disk.VHDControllerType = d.GetEnum<ControllerType>("ControllerType");
                    disk.ControllerNumber = Convert.ToInt32(d.GetProperty("ControllerNumber"));
                    disk.ControllerLocation = Convert.ToInt32(d.GetProperty("ControllerLocation"));
                    disk.Path = d.GetProperty("Path").ToString();
                    disk.Name = d.GetProperty("Name").ToString();

                    GetVirtualHardDiskDetail(powerShell, disk.Path, ref disk);

                    disks.Add(disk);
                }
            }
            return disks.ToArray();
        }

        //public static VirtualHardDiskInfo GetByPath(PowerShellManager powerShell, string vhdPath)
        //{
        //    VirtualHardDiskInfo info = null;
        //    var vmNames = new List<string>();

        //    Command cmd = new Command("Get-VM");

        //    Collection<PSObject> result = powerShell.Execute(cmd, true);

        //    if (result == null || result.Count == 0)
        //        return null;

        //    vmNames = result.Select(r => r.GetString("Name")).ToList();
        //    var drives = vmNames.SelectMany(n => Get(powerShell, n));

        //    return drives.FirstOrDefault(d=>d.Path == vhdPath);
        //}

        public static Collection<PSObject> GetPS(PowerShellManager powerShell, string vmname)
        {
            Command cmd = new Command("Get-VMHardDiskDrive");
            cmd.Parameters.Add("VMName", vmname);

            return powerShell.Execute(cmd, true);
        }

        public static void GetVirtualHardDiskDetail(PowerShellManager powerShell, string path, ref VirtualHardDiskInfo disk)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Command cmd = new Command("Get-VHD");
                cmd.Parameters.Add("Path", path);
                Collection<PSObject> result = powerShell.Execute(cmd, true);
                if (result != null && result.Count > 0)
                {
                    disk.DiskFormat = result[0].GetEnum<VirtualHardDiskFormat>("VhdFormat");
                    disk.DiskType = result[0].GetEnum<VirtualHardDiskType>("VhdType");
                    disk.ParentPath = result[0].GetProperty<string>("ParentPath");
                    disk.MaxInternalSize = Convert.ToInt64(result[0].GetProperty("Size"));
                    disk.FileSize = Convert.ToInt64(result[0].GetProperty("FileSize"));
                    disk.Attached = disk.InUse = Convert.ToBoolean(result[0].GetProperty("Attached"));
                }
            }
        }

        public static void Update(PowerShellManager powerShell, VirtualMachine vm, int newhddSize)
        {
            if (vm.Disks == null) //At this moment it isn't possible, but if somebody send vm data without vm.disks, we try to get it.
                vm.Disks = Get(powerShell, vm.Name);

            if (vm.Disks != null && vm.Disks.GetLength(0) > 0)
            {
                int oldHddSize = Convert.ToInt32(vm.Disks[0].FileSize / Constants.Size1G);
                if (newhddSize <= oldHddSize) //we can't reduce hdd size, so we just exit.
                    return;

                Command cmd = new Command("Resize-VHD");
                cmd.Parameters.Add("SizeBytes", newhddSize * Constants.Size1G);
                cmd.Parameters.Add("Path", vm.Disks[0].Path);
                powerShell.Execute(cmd, true);
            }
        }

        public static void SetIOPS(PowerShellManager powerShell, VirtualMachine vm, int minIOPS, int maxIOPS)
        {
            //TODO
            //*********Move checks in the Enterprise Server methods?*********//
            int maxPossibleIOPS = 1000000000;
            if (maxIOPS > maxPossibleIOPS)
                maxIOPS = maxPossibleIOPS;
            bool disableIOPS = (maxIOPS == 0 && minIOPS == 0);
            bool isIOPScorrect = (minIOPS <= maxIOPS) || ((minIOPS > maxIOPS) && (maxIOPS == 0));
            //***************************************************************//

            if (vm.Disks != null && isIOPScorrect)
            {
                foreach (VirtualHardDiskInfo disk in vm.Disks)
                {
                    Command cmd = new Command("Set-VMHardDiskDrive");
                    cmd.Parameters.Add("VMName", vm.Name);
                    cmd.Parameters.Add("ControllerType", disk.VHDControllerType.ToString());
                    cmd.Parameters.Add("ControllerNumber", disk.ControllerNumber);
                    cmd.Parameters.Add("ControllerLocation", disk.ControllerLocation);
                    if (disableIOPS){
                        cmd.Parameters.Add("MinimumIOPS", false);
                        cmd.Parameters.Add("MaximumIOPS", false);
                    }else{
                        cmd.Parameters.Add("MinimumIOPS", minIOPS);
                        cmd.Parameters.Add("MaximumIOPS", maxIOPS);
                    }
                    powerShell.Execute(cmd, true, true);
                }
            }
        }

        public static void Delete(PowerShellManager powerShell, VirtualHardDiskInfo[] disks)
        {
            Delete(powerShell, disks, null);
        }

        public static void Delete(PowerShellManager powerShell, VirtualHardDiskInfo[] disks, string serverNameSettings)
        {
            if (disks != null && disks.GetLength(0) > 0)
            {
                foreach (VirtualHardDiskInfo diskItem in disks)
                {
                    VirtualHardDiskInfo disk = diskItem;
                    do
                    {
                        if (!string.IsNullOrEmpty(serverNameSettings))
                        {
                            string cmd = "Invoke-Command -ComputerName " + serverNameSettings + " -ScriptBlock { Remove-item -path " + disk.Path + " }";
                            powerShell.Execute(new Command(cmd, true), false);
                        }
                        else
                        {
                            Command cmd = new Command("Remove-item");
                            cmd.Parameters.Add("path", disk.Path);
                            powerShell.Execute(cmd, false);
                        }
                        // remove all parent disks
                        disk.Path = disk.ParentPath;
                        if (!String.IsNullOrEmpty(disk.Path)) GetVirtualHardDiskDetail(powerShell, disk.Path, ref disk);
                    } while (!String.IsNullOrEmpty(disk.Path));
                }
            }
        }
    }
}
