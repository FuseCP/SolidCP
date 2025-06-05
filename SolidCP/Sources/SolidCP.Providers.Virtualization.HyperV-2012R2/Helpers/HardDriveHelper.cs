using Microsoft.Management.Infrastructure;
using SolidCP.Providers.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    public class HardDriveHelper
    {
        private PowerShellManager _powerShell;
        private MiManager _mi;

        public HardDriveHelper(PowerShellManager powerShellManager, MiManager mi)
        {
            _powerShell = powerShellManager;
            _mi = mi;
        }

        public VirtualHardDiskInfo[] Get(string vmname) //TODO: can be a bottleneck with PowerShell? Need to check it, especally when calls from HyperV.GetHddUsagesFromKVPHyperV()
        {
            List<VirtualHardDiskInfo> disks = new List<VirtualHardDiskInfo>();

            Collection<PSObject> result = GetPS(vmname);

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

                    GetVirtualHardDiskDetail(disk.Path, ref disk);

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

        public Collection<PSObject> GetPS(string vmname)
        {
            Command cmd = new Command("Get-VMHardDiskDrive");
            cmd.Parameters.Add("VMName", vmname);

            return _powerShell.Execute(cmd, true);
        }

        public void GetVirtualHardDiskDetail(string path, ref VirtualHardDiskInfo disk)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Command cmd = new Command("Get-VHD");
                cmd.Parameters.Add("Path", path);
                Collection<PSObject> result = _powerShell.Execute(cmd, true);
                if (result != null && result.Count > 0)
                {
                    disk.DiskFormat = result[0].GetEnum<VirtualHardDiskFormat>("VhdFormat");
                    disk.DiskType = result[0].GetEnum<VirtualHardDiskType>("VhdType");
                    disk.ParentPath = result[0].GetProperty<string>("ParentPath");
                    disk.MaxInternalSize = Convert.ToInt64(result[0].GetProperty("Size"));
                    disk.FileSize = Convert.ToInt64(result[0].GetProperty("FileSize"));
                    disk.Attached = disk.InUse = Convert.ToBoolean(result[0].GetProperty("Attached"));
                    disk.BlockSizeBytes = Convert.ToUInt32(result[0].GetProperty("BlockSize"));
                }
            }
        }

        public void Update(VirtualMachine realVm, VirtualMachine vmSettings)
        {
            if (realVm.Disks == null) //At this moment it isn't possible, but if somebody send vm data without vm.disks, we try to get it.
                realVm.Disks = Get(realVm.Name);

            bool vhdChanged = false;

            // remove VHD check
            if (realVm.Disks.Length > 1)
            {
                for (int i = 1; i < realVm.Disks.Length; i++)
                {
                    VirtualHardDiskInfo disk = GetParentVHD(realVm.Disks[i]);
                    bool remove = true;
                    foreach (string path in vmSettings.VirtualHardDrivePath)
                    {
                        if (path != null && disk.Path != null && Path.GetFileName(path).ToLower().Equals(Path.GetFileName(disk.Path).ToLower()))
                        {
                            remove = false;
                            break;
                        }
                    }
                    if (remove)
                    {
                        Command cmd = new Command("Remove-VMHardDiskDrive");
                        cmd.Parameters.Add("VMName", realVm.Name);
                        cmd.Parameters.Add("ControllerType", realVm.Disks[i].VHDControllerType.ToString());
                        cmd.Parameters.Add("ControllerNumber", realVm.Disks[i].ControllerNumber);
                        cmd.Parameters.Add("ControllerLocation", realVm.Disks[i].ControllerLocation);
                        _powerShell.Execute(cmd, true, true);
                        vhdChanged = true;
                        Delete(realVm.Disks[i]);
                    }
                }
            }

            // add VHD check
            if (vmSettings.VirtualHardDrivePath.Length > 1)
            {
                for (int i = 1; i < vmSettings.VirtualHardDrivePath.Length; i++)
                {
                    bool add = true;
                    if (String.IsNullOrEmpty(vmSettings.VirtualHardDrivePath[i]))
                    {
                        int index = 1;
                        string msHddHyperVFolderName = "Virtual Hard Disks\\" + vmSettings.Name;
                        while (String.IsNullOrEmpty(vmSettings.VirtualHardDrivePath[i]))
                        {
                            bool addPath = true;
                            foreach (string path in vmSettings.VirtualHardDrivePath)
                            {
                                if (path != null && path.ToLower().Contains((vmSettings.Name + index.ToString() + Path.GetExtension(vmSettings.OperatingSystemTemplatePath)).ToLower()))
                                {
                                    addPath = false;
                                    index++;
                                    break;
                                }
                            }
                            if (addPath)
                            {
                                vmSettings.VirtualHardDrivePath[i] = Path.Combine(vmSettings.RootFolderPath, msHddHyperVFolderName + index.ToString() + Path.GetExtension(vmSettings.OperatingSystemTemplatePath));
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int t = 0; t < realVm.Disks.Length; t++)
                        {
                            VirtualHardDiskInfo disk = GetParentVHD(realVm.Disks[t]);
                            if (disk.Path != null && Path.GetFileName(vmSettings.VirtualHardDrivePath[i]).ToLower().Equals(Path.GetFileName(disk.Path).ToLower()))
                            {
                                add = false;
                                break;
                            }
                        }
                    }
                    if (add)
                    {
                        VirtualHardDiskInfo disk = GetParentVHD(realVm.Disks[0]);
                        CreateVirtualHardDisk(vmSettings.VirtualHardDrivePath[i], disk.DiskType, disk.BlockSizeBytes, (ulong)vmSettings.HddSize[i]);
                        Command cmd = new Command("Add-VMHardDiskDrive");
                        cmd.Parameters.Add("VMName", realVm.Name);
                        cmd.Parameters.Add("Path", vmSettings.VirtualHardDrivePath[i]);
                        cmd.Parameters.Add("ControllerType", realVm.Disks[0].VHDControllerType.ToString());
                        _powerShell.Execute(cmd, true, true);
                        vhdChanged = true;
                    }
                }
            }

            // resize VHD check
            if (vhdChanged) realVm.Disks = Get(realVm.Name);
            if (realVm.Disks != null)
            {
                for (int i = 0; i < realVm.Disks.Length; i++)
                {
                    int oldHddSize = Convert.ToInt32(realVm.Disks[i].FileSize / Constants.Size1G);
                    int newhddSize = 0;
                    VirtualHardDiskInfo disk = GetParentVHD(realVm.Disks[i]);
                    if (i == 0)
                    {
                        newhddSize = vmSettings.HddSize[0];
                    }
                    else
                    {
                        if (disk.Path != null)
                        {
                            for (int t = 0; t < vmSettings.VirtualHardDrivePath.Length; t++)
                            {
                                if (vmSettings.VirtualHardDrivePath[t] != null && Path.GetFileName(disk.Path).ToLower().Equals(Path.GetFileName(vmSettings.VirtualHardDrivePath[t]).ToLower()))
                                {
                                    newhddSize = vmSettings.HddSize[t];
                                }
                            }
                        }
                    }
                    if (newhddSize <= oldHddSize) //we can't reduce hdd size, so we just exit.
                        continue;

                    Command cmd = new Command("Resize-VHD");
                    cmd.Parameters.Add("SizeBytes", newhddSize * Constants.Size1G);
                    cmd.Parameters.Add("Path", realVm.Disks[i].Path);
                    _powerShell.Execute(cmd, true);
                }
            }
        }

        private VirtualHardDiskInfo GetParentVHD(VirtualHardDiskInfo disk)
        {
            VirtualHardDiskInfo resDisk = disk.Clone();
            while (!String.IsNullOrEmpty(resDisk.ParentPath))
            {
                resDisk.Path = resDisk.ParentPath;
                GetVirtualHardDiskDetail(resDisk.Path, ref resDisk);
            }
            return resDisk;
        }

        public CimMethodResult CreateVirtualHardDisk(string destinationPath, VirtualHardDiskType diskType, uint blockSizeBytes, UInt64 sizeGB)
        {
            string destFolder = Path.GetDirectoryName(destinationPath);
            if (!DirectoryExists(destFolder)) CreateFolder(destFolder);

            destinationPath = FileUtils.EvaluateSystemVariables(destinationPath);

            string fileExtension = Path.GetExtension(destinationPath);
            VirtualHardDiskFormat format = fileExtension.Equals(".vhdx", StringComparison.InvariantCultureIgnoreCase) ? VirtualHardDiskFormat.VHDX : VirtualHardDiskFormat.VHD;

            CimInstance imageService = _mi.GetCimInstance("Msvm_ImageManagementService");
            CimClass settingsClass = _mi.GetCimClass("Msvm_VirtualHardDiskSettingData");

            CimInstance settingsInstance = new CimInstance(settingsClass);
            settingsInstance.CimInstanceProperties["MaxInternalSize"].Value = sizeGB * Constants.Size1G;
            settingsInstance.CimInstanceProperties["Path"].Value = destinationPath;
            settingsInstance.CimInstanceProperties["Type"].Value = diskType;
            settingsInstance.CimInstanceProperties["Format"].Value = format;
            settingsInstance.CimInstanceProperties["BlockSize"].Value = (blockSizeBytes > 0) ? blockSizeBytes : 0;

            var inParams = new CimMethodParametersCollection
                {
                    CimMethodParameter.Create(
                        "VirtualDiskSettingData",
                        _mi.SerializeToCimDtd20(settingsInstance),
                        Microsoft.Management.Infrastructure.CimType.String,
                        CimFlags.None)
                };

            return _mi.InvokeMethod(imageService, "CreateVirtualHardDisk", inParams);


            //Command cmd = new Command("New-VHD");

            //cmd.Parameters.Add("SizeBytes", sizeGB * Constants.Size1G);
            //cmd.Parameters.Add("Path", destinationPath);
            //cmd.Parameters.Add(diskType.ToString());
            //if (blockSizeBytes > 0) cmd.Parameters.Add("BlockSizeBytes", blockSizeBytes);
            //return powerShellwithJobs.TryExecuteAsJob(cmd, true);
        }

        private void CreateFolder(string path)
        {
            string serverNameSettings = _powerShell.RemoteComputerName; //TODO: remove
            VdsHelper.ExecuteRemoteProcess(serverNameSettings, String.Format("cmd.exe /c md \"{0}\"", path));
        }

        private bool DirectoryExists(string path)
        {
            if (path.StartsWith(@"\\")) // network share
                                        // TODO: That won't work with remote HyperV, unless network share added into domain.
                                        // Need to check remotly
                return Directory.Exists(path);
            else
            {
                using (var mi = new MiManager(_mi, Constants.WMI_CIMV2_NAMESPACE)) //because change namespace
                {
                    CimInstance objDir = mi.GetCimInstance("Win32_Directory", "Name='{0}'", path.Replace("\\", "\\\\"));
                    return (objDir != null);
                }
            }
        }

        public void SetIOPS(VirtualMachine vm, int minIOPS, int maxIOPS)
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
                    if (disableIOPS)
                    {
                        cmd.Parameters.Add("MinimumIOPS", false);
                        cmd.Parameters.Add("MaximumIOPS", false);
                    }
                    else
                    {
                        cmd.Parameters.Add("MinimumIOPS", minIOPS);
                        cmd.Parameters.Add("MaximumIOPS", maxIOPS);
                    }
                    _powerShell.Execute(cmd, true, true);
                }
            }
        }

        public void Delete(VirtualHardDiskInfo[] disks)
        {
            if (disks != null && disks.GetLength(0) > 0)
            {
                foreach (VirtualHardDiskInfo diskItem in disks)
                {
                    VirtualHardDiskInfo disk = diskItem;
                    Delete(disk);
                }
            }
        }

        private void Delete(VirtualHardDiskInfo disk)
        {
            do
            {
                Command cmd = new Command("Remove-item");
                cmd.Parameters.Add("path", disk.Path);
                string serverNameSettings = _powerShell.RemoteComputerName;

                if (!string.IsNullOrEmpty(serverNameSettings))
                {
                    //string cmdScript = "Invoke-Command -ComputerName " + serverNameSettings + " -ScriptBlock { Remove-item -path \"" + disk.Path + "\" }";
                    string cmdScript = "Invoke-Command -ComputerName '" + serverNameSettings + "' -ScriptBlock { " + _powerShell.ConvertCommandAsScript(cmd) + " }";
                    _powerShell.Execute(new Command(cmdScript, true), false, true); //throw an exception that should help set up the connection to the remote server.
                }
                else
                {   
                    _powerShell.Execute(cmd, false);
                }
                // remove all parent disks
                disk.Path = disk.ParentPath;
                if (!String.IsNullOrEmpty(disk.Path)) GetVirtualHardDiskDetail(disk.Path, ref disk);
            } while (!String.IsNullOrEmpty(disk.Path));
        }
    }
}
