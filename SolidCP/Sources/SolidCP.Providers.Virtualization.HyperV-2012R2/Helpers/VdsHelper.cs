using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Management;
using System.Threading;
using Microsoft.Storage.Vds;
using Microsoft.Storage.Vds.Advanced;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Virtualization.Extensions;
using Path = System.IO.Path;

namespace SolidCP.Providers.Virtualization
{
    public static class VdsHelper
    {
        public static MountedDiskInfo GetMountedDiskInfo(string serverName, int driveNumber)
        {
            MountedDiskInfo diskInfo = new MountedDiskInfo { DiskNumber = driveNumber };

            // find mounted disk using VDS
            AdvancedDisk advancedDisk = null;
            Pack diskPack = null;

            // first attempt
            Thread.Sleep(3000);
            HostedSolutionLog.LogInfo("Trying to find mounted disk - first attempt");
            FindVdsDisk(serverName, diskInfo.DiskNumber, out advancedDisk, out diskPack);

            // second attempt
            if (advancedDisk == null)
            {
                Thread.Sleep(20000);
                HostedSolutionLog.LogInfo("Trying to find mounted disk - second attempt");
                FindVdsDisk(serverName, diskInfo.DiskNumber, out advancedDisk, out diskPack);
            }

            if (advancedDisk == null)
                throw new Exception("Could not find mounted disk");

            // Set disk address
            diskInfo.DiskAddress = advancedDisk.DiskAddress;
            var addressParts = diskInfo.DiskAddress.ParseExact("Port{0}Path{1}Target{2}Lun{3}");
            var portNumber = addressParts[0];
            var targetId = addressParts[2];
            var lun = addressParts[3];

            // check if DiskPart must be used to bring disk online and clear read-only flag
            bool useDiskPartToClearReadOnly = false;
            if (ConfigurationManager.AppSettings[Constants.CONFIG_USE_DISKPART_TO_CLEAR_READONLY_FLAG] != null)
                useDiskPartToClearReadOnly = Boolean.Parse(ConfigurationManager.AppSettings[Constants.CONFIG_USE_DISKPART_TO_CLEAR_READONLY_FLAG]);

            // determine disk index for DiskPart
            Wmi cimv2 = new Wmi(serverName, Constants.WMI_CIMV2_NAMESPACE);
            ManagementObject objDisk = cimv2.GetWmiObject("win32_diskdrive",
                "Model='Msft Virtual Disk SCSI Disk Device' and ScsiTargetID={0} and ScsiLogicalUnit={1} and scsiPort={2}",
                targetId, lun, portNumber);

            if (useDiskPartToClearReadOnly)
            {
                // *** Clear Read-Only and bring disk online with DiskPart ***
                HostedSolutionLog.LogInfo("Clearing disk Read-only flag and bringing disk online");

                if (objDisk != null)
                {
                    // disk found
                    // run DiskPart
                    string diskPartResult = RunDiskPart(serverName, String.Format(@"select disk {0}
attributes disk clear readonly
online disk
exit", Convert.ToInt32(objDisk["Index"])));

                    HostedSolutionLog.LogInfo("DiskPart Result: " + diskPartResult);
                }
            }
            else
            {
                // *** Clear Read-Only and bring disk online with VDS ***
                // clear Read-Only
                if ((advancedDisk.Flags & DiskFlags.ReadOnly) == DiskFlags.ReadOnly)
                {
                    HostedSolutionLog.LogInfo("Clearing disk Read-only flag");
                    advancedDisk.ClearFlags(DiskFlags.ReadOnly);
                    while ((advancedDisk.Flags & DiskFlags.ReadOnly) == DiskFlags.ReadOnly)
                    {
                        Thread.Sleep(100);
                        advancedDisk.Refresh();
                    }
                }

                // bring disk ONLINE
                if (advancedDisk.Status == DiskStatus.Offline)
                {
                    HostedSolutionLog.LogInfo("Bringing disk online");
                    advancedDisk.Online();
                    while (advancedDisk.Status == DiskStatus.Offline)
                    {
                        Thread.Sleep(100);
                        advancedDisk.Refresh();
                    }
                }
            }

            // small pause after getting disk online
            Thread.Sleep(3000);

            // get disk again
            FindVdsDisk(serverName, diskInfo.DiskNumber, out advancedDisk, out diskPack);

            // find volumes using VDS
            List<string> volumes = new List<string>();
            HostedSolutionLog.LogInfo("Querying disk volumes with VDS");
            foreach (Volume volume in diskPack.Volumes)
            {
                string letter = volume.DriveLetter.ToString();
                if (letter != "")
                    volumes.Add(letter);
            }

            // find volumes using WMI
            if (volumes.Count == 0 && objDisk != null)
            {
                HostedSolutionLog.LogInfo("Querying disk volumes with WMI");
                foreach (ManagementObject objPartition in objDisk.GetRelated("Win32_DiskPartition"))
                {
                    foreach (ManagementObject objVolume in objPartition.GetRelated("Win32_LogicalDisk"))
                    {
                        volumes.Add(objVolume["Name"].ToString().TrimEnd(':'));
                    }
                }
            }

            HostedSolutionLog.LogInfo("Volumes found: " + volumes.Count);

            // Set volumes
            diskInfo.DiskVolumes = volumes.ToArray();

            return diskInfo;
        }

        public static void FindVdsDisk(string serverName, int driveNumber, out AdvancedDisk advancedDisk, out Pack diskPack)
        {
            Func<Disk, bool> compareFunc =  disk => disk.Name.EndsWith("PhysicalDrive" + driveNumber);
            FindVdsDisk(serverName, compareFunc, out advancedDisk, out diskPack);
        }
        public static void FindVdsDisk(string serverName, string diskAddress, out AdvancedDisk advancedDisk, out Pack diskPack)
        {
            Func<Disk, bool> compareFunc =  disk => disk.DiskAddress == diskAddress;
            FindVdsDisk(serverName, compareFunc, out advancedDisk, out diskPack);
        }

        private static void FindVdsDisk(string serverName, Func<Disk, bool> compareFunc, out AdvancedDisk advancedDisk, out Pack diskPack)
        {
            advancedDisk = null;
            diskPack = null;

            ServiceLoader serviceLoader = new ServiceLoader();
            Service vds = serviceLoader.LoadService(serverName);
            vds.WaitForServiceReady();

            foreach (Disk disk in vds.UnallocatedDisks)
            {
                if (compareFunc(disk))
                {
                    advancedDisk = (AdvancedDisk) disk;
                    break;
                }
            }

            if (advancedDisk == null)
            {
                vds.HardwareProvider = false;
                vds.SoftwareProvider = true;

                foreach (SoftwareProvider provider in vds.Providers)
                    foreach (Pack pack in provider.Packs)
                        foreach (Disk disk in pack.Disks)
                            if (compareFunc(disk))
                            {
                                diskPack = pack;
                                advancedDisk = (AdvancedDisk) disk;
                                break;
                            }
            }
        }

        // obsolete and currently is not used
        private static string RunDiskPart(string serverName, string script)
        {
            // create temp script file name
            string localPath = Path.Combine(GetTempRemoteFolder(serverName), Guid.NewGuid().ToString("N"));

            // save script to remote temp file
            string remotePath = ConvertToUNC(serverName, localPath);
            File.AppendAllText(remotePath, script);

            // run diskpart
            ExecuteRemoteProcess(serverName, "DiskPart /s " + localPath);

            // delete temp script
            try
            {
                File.Delete(remotePath);
            }
            catch
            {
                // TODO
            }

            return "";
        }

        public static string ConvertToUNC(string serverName, string path)
        {
            if (String.IsNullOrEmpty(serverName)
                || path.StartsWith(@"\\"))
                return path;

            return String.Format(@"\\{0}\{1}", serverName, path.Replace(":", "$"));
        }

        public static string GetTempRemoteFolder(string serverName)
        {
            Wmi cimv2 = new Wmi(serverName, "root\\cimv2");
            ManagementObject objOS = cimv2.GetWmiObject("win32_OperatingSystem");
            string sysPath = (string)objOS["SystemDirectory"];

            // remove trailing slash
            if (sysPath.EndsWith("\\"))
                sysPath = sysPath.Substring(0, sysPath.Length - 1);

            sysPath = sysPath.Substring(0, sysPath.LastIndexOf("\\") + 1) + "Temp";

            return sysPath;
        }

        public static void ExecuteRemoteProcess(string serverName, string command)
        {
            Wmi cimv2 = new Wmi(serverName, "root\\cimv2");
            ManagementClass objProcess = cimv2.GetWmiClass("Win32_Process");

            // run process
            object[] methodArgs = { command, null, null, 0 };
            objProcess.InvokeMethod("Create", methodArgs);

            // process ID
            int processId = Convert.ToInt32(methodArgs[3]);

            // wait until finished
            // Create event query to be notified within 1 second of 
            // a change in a service
            WqlEventQuery query =
                new WqlEventQuery("__InstanceDeletionEvent",
                new TimeSpan(0, 0, 1),
                "TargetInstance isa \"Win32_Process\"");

            // Initialize an event watcher and subscribe to events 
            // that match this query
            ManagementEventWatcher watcher = new ManagementEventWatcher(cimv2.GetScope(), query);
            // times out watcher.WaitForNextEvent in 20 seconds
            watcher.Options.Timeout = new TimeSpan(0, 0, 20);

            // Block until the next event occurs 
            // Note: this can be done in a loop if waiting for 
            //        more than one occurrence
            while (true)
            {
                ManagementBaseObject e = null;

                try
                {
                    // wait untill next process finish
                    e = watcher.WaitForNextEvent();
                }
                catch
                {
                    // nothing has been finished in timeout period
                    return; // exit
                }

                // check process id
                int pid = Convert.ToInt32(((ManagementBaseObject)e["TargetInstance"])["ProcessID"]);
                if (pid == processId)
                {
                    //Cancel the subscription
                    watcher.Stop();

                    // exit
                    return;
                }
            }
        }
    }
}
