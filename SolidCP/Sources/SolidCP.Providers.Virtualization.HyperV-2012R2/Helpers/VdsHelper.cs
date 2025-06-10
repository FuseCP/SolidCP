using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;
using Microsoft.Storage.Vds;
using Microsoft.Storage.Vds.Advanced;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Virtualization.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Path = System.IO.Path;


namespace SolidCP.Providers.Virtualization
{
    public class VdsHelper
    {
        private MiManager _miCim;
        private FileSystemHelper _fileSystemHelper;
        private string _serverName;

        public VdsHelper(MiManager mi, FileSystemHelper fileSystemHelper)
        {
            _serverName = mi.TargetComputer;
            _miCim = new MiManager(mi, Constants.WMI_CIMV2_NAMESPACE);
            _fileSystemHelper = fileSystemHelper;
        }

        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            // find mounted disk using VDS
            AdvancedDisk advancedDisk = null;
            Pack diskPack = null;

            FindVdsDisk(diskAddress, out advancedDisk, out diskPack);

            if (advancedDisk == null)
                throw new Exception("Could not find mounted disk");

            // find volume
            Volume diskVolume = null;
            foreach (Volume volume in diskPack.Volumes)
            {
                if (volume.DriveLetter.ToString() == volumeName)
                {
                    diskVolume = volume;
                    break;
                }
            }

            if (diskVolume == null)
                throw new Exception("Could not find disk volume: " + volumeName);

            // determine maximum available space
            ulong oneMegabyte = 1048576;
            ulong freeSpace = 0;
            foreach (DiskExtent extent in advancedDisk.Extents)
            {
                if (extent.Type != Microsoft.Storage.Vds.DiskExtentType.Free)
                    continue;

                if (extent.Size > oneMegabyte)
                    freeSpace += extent.Size;
            }

            if (freeSpace == 0)
                return;

            // input disk
            InputDisk inputDisk = new InputDisk();
            foreach (VolumePlex plex in diskVolume.Plexes)
            {
                inputDisk.DiskId = advancedDisk.Id;
                inputDisk.Size = freeSpace;
                inputDisk.PlexId = plex.Id;

                foreach (DiskExtent extent in plex.Extents)
                    inputDisk.MemberIndex = extent.MemberIndex;

                break;
            }

            // extend volume
            Async extendEvent = diskVolume.BeginExtend(new InputDisk[] { inputDisk }, null, null);
            while (!extendEvent.IsCompleted)
                System.Threading.Thread.Sleep(100);
            diskVolume.EndExtend(extendEvent);
        }

        public MountedDiskInfo GetMountedDiskInfo(int driveNumber)
        {
            MountedDiskInfo diskInfo = new MountedDiskInfo { DiskNumber = driveNumber };

            // find mounted disk using VDS
            AdvancedDisk advancedDisk = null;
            Pack diskPack = null;

            // first attempt
            Thread.Sleep(3000);
            HostedSolutionLog.LogInfo("Trying to find mounted disk - first attempt");
            FindVdsDisk(diskInfo.DiskNumber, out advancedDisk, out diskPack);

            // second attempt
            if (advancedDisk == null)
            {
                Thread.Sleep(20000);
                HostedSolutionLog.LogInfo("Trying to find mounted disk - second attempt");
                FindVdsDisk(diskInfo.DiskNumber, out advancedDisk, out diskPack);
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
            CimInstance objDisk = _miCim.GetCimInstanceWithSelect(
                    "win32_diskdrive",
                    "Index", // Select only "Index"
                    "Model='Msft Virtual Disk SCSI Disk Device' and ScsiTargetID={0} and ScsiLogicalUnit={1} and scsiPort={2}",
                    targetId, lun, portNumber
                );

            if (useDiskPartToClearReadOnly)
            {
                // *** Clear Read-Only and bring disk online with DiskPart ***
                HostedSolutionLog.LogInfo("Clearing disk Read-only flag and bringing disk online");

                if (objDisk != null && objDisk.CimInstanceProperties["Index"]?.Value != null)
                {
                    // disk found
                    // run DiskPart
                    string diskPartResult = RunDiskPart(
                        String.Format(@"select disk {0}
                                        attributes disk clear readonly
                                        online disk
                                        exit", Convert.ToInt32(objDisk.CimInstanceProperties["Index"].Value)));

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
            FindVdsDisk(diskInfo.DiskNumber, out advancedDisk, out diskPack);

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
                var partitions = _miCim.EnumerateAssociatedInstances(objDisk, "Win32_DiskDriveToDiskPartition", "Win32_DiskPartition");
                foreach (var objPartition in partitions)
                {
                    var logicalDisks = _miCim.EnumerateAssociatedInstances(objPartition, "Win32_LogicalDiskToPartition", "Win32_LogicalDisk");
                    foreach (var objVolume in logicalDisks)
                    {
                        if (objVolume.CimInstanceProperties["Name"]?.Value != null)
                        {
                            volumes.Add(objVolume.CimInstanceProperties["Name"].Value.ToString().TrimEnd(':'));
                        }
                    }
                }
            }

            HostedSolutionLog.LogInfo("Volumes found: " + volumes.Count);

            // Set volumes
            diskInfo.DiskVolumes = volumes.ToArray();

            return diskInfo;
        }

        public void FindVdsDisk(int driveNumber, out AdvancedDisk advancedDisk, out Pack diskPack)
        {
            Func<Disk, bool> compareFunc =  disk => disk.Name.EndsWith("PhysicalDrive" + driveNumber);
            FindVdsDisk(compareFunc, out advancedDisk, out diskPack);
        }

        public void FindVdsDisk(string diskAddress, out AdvancedDisk advancedDisk, out Pack diskPack)
        {
            Func<Disk, bool> compareFunc =  disk => disk.DiskAddress == diskAddress;
            FindVdsDisk(compareFunc, out advancedDisk, out diskPack);
        }

        private void FindVdsDisk(Func<Disk, bool> compareFunc, out AdvancedDisk advancedDisk, out Pack diskPack)
        {
            advancedDisk = null;
            diskPack = null;

            ServiceLoader serviceLoader = new ServiceLoader();
            Service vds = serviceLoader.LoadService(_serverName);
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
        private string RunDiskPart(string script)
        {
            // create temp script file name
            string localPath = Path.Combine(_fileSystemHelper.GetTempRemoteFolder(), Guid.NewGuid().ToString("N"));

            // save script to remote temp file
            string remotePath = _fileSystemHelper.ConvertToUNC(localPath);
            File.AppendAllText(remotePath, script);

            // run diskpart
            _fileSystemHelper.ExecuteRemoteProcess("DiskPart /s " + localPath);

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

        //public static void ExecuteRemoteProcess(string serverName, string command)
        //{
        //    Wmi cimv2 = new Wmi(serverName, "root\\cimv2");
        //    ManagementClass objProcess = cimv2.GetWmiClass("Win32_Process");

        //    // run process
        //    object[] methodArgs = { command, null, null, 0 };
        //    objProcess.InvokeMethod("Create", methodArgs);

        //    // process ID
        //    int processId = Convert.ToInt32(methodArgs[3]);

        //    // wait until finished
        //    // Create event query to be notified within 1 second of 
        //    // a change in a service
        //    WqlEventQuery query =
        //        new WqlEventQuery("__InstanceDeletionEvent",
        //        new TimeSpan(0, 0, 1),
        //        "TargetInstance isa \"Win32_Process\"");

        //    // Initialize an event watcher and subscribe to events 
        //    // that match this query
        //    ManagementEventWatcher watcher = new ManagementEventWatcher(cimv2.GetScope(), query);
        //    // times out watcher.WaitForNextEvent in 20 seconds
        //    watcher.Options.Timeout = new TimeSpan(0, 0, 20);

        //    // Block until the next event occurs 
        //    // Note: this can be done in a loop if waiting for 
        //    //        more than one occurrence
        //    while (true)
        //    {
        //        ManagementBaseObject e = null;

        //        try
        //        {
        //            // wait untill next process finish
        //            e = watcher.WaitForNextEvent();
        //        }
        //        catch
        //        {
        //            // nothing has been finished in timeout period
        //            return; // exit
        //        }

        //        // check process id
        //        int pid = Convert.ToInt32(((ManagementBaseObject)e["TargetInstance"])["ProcessID"]);
        //        if (pid == processId)
        //        {
        //            //Cancel the subscription
        //            watcher.Stop();

        //            // exit
        //            return;
        //        }
        //    }
        //}
    }
}
