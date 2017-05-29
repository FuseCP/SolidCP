// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  SolidCP  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

using System.Reflection;
using System.Globalization;

using System.Xml;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

using Vds = Microsoft.Storage.Vds;
using System.Configuration;
﻿using System.Linq;
﻿using SolidCP.Providers.Virtualization.Extensions;

using Microsoft.SystemCenter.VirtualMachineManager;
using Microsoft.SystemCenter.VirtualMachineManager.Remoting;
using Microsoft.SystemCenter.VirtualMachineManager.Cmdlets;
using Microsoft.VirtualManager.PowerShellAbstractionLayer;


namespace SolidCP.Providers.Virtualization
{
    public class HyperVvmm : HostingServiceProviderBase, IVirtualizationServer2012
    {
        #region Provider Settings
        protected string ServerNameSettings
        {
            get { return ProviderSettings["ServerName"]; }
        }

        public int AutomaticStartActionSettings
        {
            get { return ProviderSettings.GetInt("StartAction"); }
        }

        public int AutomaticStartupDelaySettings
        {
            get { return ProviderSettings.GetInt("StartupDelay"); }
        }

        public int AutomaticStopActionSettings
        {
            get { return ProviderSettings.GetInt("StopAction"); }
        }

        public int AutomaticRecoveryActionSettings
        {
            get { return 1 /* restart */; }
        }

        public int CpuReserveSettings
        {
            get { return ProviderSettings.GetInt("CpuReserve"); }
        }

        public int CpuLimitSettings
        {
            get { return ProviderSettings.GetInt("CpuLimit"); }
        }

        public int CpuWeightSettings
        {
            get { return ProviderSettings.GetInt("CpuWeight"); }
        }


        public ReplicaMode ReplicaMode
        {
            get { return (ReplicaMode) Enum.Parse(typeof(ReplicaMode) , ProviderSettings["ReplicaMode"]); }
        }
        protected string ReplicaServerPath
        {
            get { return ProviderSettings["ReplicaServerPath"]; }
        }
        protected string ReplicaServerThumbprint
        {
            get { return ProviderSettings["ReplicaServerThumbprint"]; }
        }
        #endregion

        #region Fields

        private PowerShellManager _powerShell;
        protected PowerShellManager PowerShell
        {
            get { return _powerShell ?? (_powerShell = new PowerShellManager(ServerNameSettings)); }
        }
        
        private Wmi _wmi;
        private Wmi wmi
        {
            get { return _wmi ?? (_wmi = new Wmi(ServerNameSettings, Constants.WMI_VIRTUALIZATION_NAMESPACE)); }
        }
        #endregion

        #region Constructors
        public HyperVvmm()
        {
        }
        #endregion

        #region Virtual Machines
        
        public VirtualMachine GetVirtualMachine(string vmId)
        {
            return GetVirtualMachineInternal(vmId, false);
        }
        
        public VirtualMachine GetVirtualMachineEx(string vmId)
        {
            return GetVirtualMachineInternal(vmId, true);
        }

        protected VirtualMachine GetVirtualMachineInternal(string vmId, bool extendedInfo)
        {
            HostedSolutionLog.LogStart("GetVirtualMachine");
            HostedSolutionLog.DebugInfo("Virtual Machine: {0}", vmId);

            VirtualMachine vm = new VirtualMachine();
            vm.VirtualMachineId = vmId;

            try
            {
                Command cmd = new Command("Get-SCVirtualMachine");
                cmd.Parameters.Add("Id", vmId);
                Collection<PSObject> result = PowerShell.Execute(cmd, true);

                if (result != null && result.Count > 0)
                {
                    vm.Name = result[0].GetString("Name");
                    vm.State = result[0].GetEnum<VirtualMachineState>("VirtualMachineState");
                    vm.CpuUsage = ConvertNullableToInt32(result[0].GetProperty("CPUUtilization"));
                    // This does not truly give the RAM usage, only the memory assigned to the VPS
                    // Lets handle detection of total memory and usage else where. SetUsagesFromKVP method have been made for it.
                    vm.RamUsage = Convert.ToInt32(ConvertNullableToInt64(result[0].GetProperty("MemoryAssignedMB")) / Constants.Size1M);
                    vm.RamSize = Convert.ToInt32(ConvertNullableToInt64(result[0].GetProperty("DynamicMemoryDemandMB")) / Constants.Size1M);
                    vm.Uptime = Convert.ToInt64(result[0].GetProperty<TimeSpan>("UpTime").TotalMilliseconds);
                    vm.Status = result[0].GetProperty("Status").ToString();
                    vm.Generation = result[0].GetInt("Generation");
                    vm.ProcessorCount = result[0].GetInt("CPUCount");
                    vm.ParentSnapshotId = result[0].GetString("VMCheckpoints");
                    vm.Heartbeat = VirtualMachineHelper.GetVMHeartBeatStatus(PowerShell, vm.Name);
                    vm.CreatedDate = result[0].GetProperty<DateTime>("CreationTime");
                    vm.ReplicationState = result[0].GetEnum<ReplicationState>("ReplicationState");

                    if (extendedInfo)
                    {
                        vm.CpuCores = VirtualMachineHelper.GetVMProcessors(PowerShell, vm.Name);

                        // BIOS 
                        BiosInfo biosInfo = BiosHelper.Get(PowerShell, vm.Name, vm.Generation);
                        vm.NumLockEnabled = biosInfo.NumLockEnabled;
                        vm.BootFromCD = biosInfo.BootFromCD;

                        // DVD drive
                        var dvdInfo = DvdDriveHelper.Get(PowerShell, vm.Name);
                        vm.DvdDriveInstalled = dvdInfo != null;

                        // HDD
                        vm.Disks = HardDriveHelper.Get(PowerShell, vm.Name);

                        if (vm.Disks != null && vm.Disks.GetLength(0) > 0)
                        {
                            vm.VirtualHardDrivePath = vm.Disks[0].Path;
                            vm.HddSize = Convert.ToInt32(vm.Disks[0].FileSize / Constants.Size1G);
                        }

                        // network adapters
                        vm.Adapters = NetworkAdapterHelper.Get(PowerShell, vm.Name);
                    }

                    vm.DynamicMemory = MemoryHelper.GetDynamicMemory(PowerShell, vm.Name);

                    // If it is possible get usage ram and usage hdd data from KVP
                    SetUsagesFromKVP(ref vm);
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetVirtualMachine", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("GetVirtualMachine");
            return vm;
        }

        public List<VirtualMachine> GetVirtualMachines()
        {
            HostedSolutionLog.LogStart("GetVirtualMachines");

            List<VirtualMachine> vmachines = new List<VirtualMachine>();

            try
            {
                HostedSolutionLog.LogInfo("Before Get-SCVirtualMachine command");

                Command cmd = new Command("Get-SCVirtualMachine");
                Collection<PSObject> result = PowerShell.Execute(cmd, true);

                HostedSolutionLog.LogInfo("After Get-SCVirtualMachine command");
                foreach (PSObject current in result)
                {
                    HostedSolutionLog.LogInfo("- start VM -");
                    var vm = new VirtualMachine();
                    HostedSolutionLog.LogInfo("create");
                    vm.VirtualMachineId = current.GetProperty("ID").ToString();
                    HostedSolutionLog.LogInfo("VirtualMachineId {0}", vm.VirtualMachineId);
                    vm.Name = current.GetString("Name");
                    HostedSolutionLog.LogInfo("Name {0}", vm.Name);
                    vm.State = current.GetEnum<VirtualMachineState>("State");
                    HostedSolutionLog.LogInfo("State {0}", vm.State);
                    vm.Uptime = Convert.ToInt64(current.GetProperty<TimeSpan>("UpTime").TotalMilliseconds);
                    HostedSolutionLog.LogInfo("Uptime {0}", vm.Uptime);
                    vm.ReplicationState = current.GetEnum<ReplicationState>("ReplicationState");
                    HostedSolutionLog.LogInfo("ReplicationState {0}", vm.ReplicationState);
                    vmachines.Add(vm);
                    HostedSolutionLog.LogInfo("- end VM -");
                }
                HostedSolutionLog.LogInfo("Finish");
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetVirtualMachines", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("GetVirtualMachines");
            return vmachines;

        }

        public byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
        {
            ManagementBaseObject objSummary = GetVirtualMachineSummaryInformation(vmId, (SummaryInformationRequest)size);
            wmi.Dump(objSummary);
            return GetTumbnailFromSummaryInformation(objSummary, size);
            //return (byte[]) (new ImageConverter()).ConvertTo(new Bitmap(80, 60), typeof (byte[]));
        }

        private byte[] GetTumbnailFromSummaryInformation(ManagementBaseObject objSummary, ThumbnailSize size)
        {
            int width = 80;
            int height = 60;

            if (size == ThumbnailSize.Medium160x120)
            {
                width = 160;
                height = 120;
            }
            else if (size == ThumbnailSize.Large320x240)
            {
                width = 320;
                height = 240;
            }

            byte[] imgData = (byte[])objSummary["ThumbnailImage"];

            // create new bitmap
            Bitmap bmp = new Bitmap(width, height);

            if (imgData != null)
            {
                // lock bitmap
                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb565);

                // get address of the first line
                IntPtr ptr = bmpData.Scan0;

                // coby thumbnail bytes into bitmap
                System.Runtime.InteropServices.Marshal.Copy(imgData, 0, ptr, imgData.Length);

                // unlock image
                bmp.UnlockBits(bmpData);
            }
            else
            {
                // fill grey rectangle
                Graphics g = Graphics.FromImage(bmp);
                SolidBrush brush = new SolidBrush(Color.LightGray);
                g.FillRectangle(brush, 0, 0, width, height);
            }

            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Png);

            stream.Flush();
            byte[] buffer = stream.ToArray();

            bmp.Dispose();
            stream.Dispose();

            return buffer;
        }

        public VirtualMachine CreateVirtualMachine(VirtualMachine vm)
        {
            // evaluate paths
            vm.RootFolderPath = FileUtils.EvaluateSystemVariables(vm.RootFolderPath);
            vm.OperatingSystemTemplatePath = FileUtils.EvaluateSystemVariables(vm.OperatingSystemTemplatePath);
            vm.VirtualHardDrivePath = FileUtils.EvaluateSystemVariables(vm.VirtualHardDrivePath);
            
            try
            {

                // Get the VMM Server
                Command cmdvmm = new Command("Get-VMMServer");
                cmdvmm.Parameters.Add("Name", vm.OperatingSystemTemplate);
                Collection<PSObject> ActiveVMMServer = PowerShell.Execute(cmdvmm, false, true);



                // Get the VM Template Ver
                Command cmdvmtmp = new Command("Get-SCVMTemplate");
                cmdvmtmp.Parameters.Add("Name", vm.OperatingSystemTemplate);
                cmdvmtmp.Parameters.Add("VMMServer", ActiveVMMServer);
                Collection<PSObject> VMTemplate = PowerShell.Execute(cmdvmtmp, false, true);

                // Select VMHost
                Command cmdvmhosts = new Command("Get-SCVMHost");
                cmdvmtmp.Parameters.Add("VMMServer", ActiveVMMServer);
                Collection<PSObject> vmhosts = PowerShell.Execute(cmdvmhosts, false, true);
                Command cmdrandomhost = new Command("Get-Random");
                cmdrandomhost.Parameters.Add("InputObject", vmhosts);
                Collection<PSObject> vmhost = PowerShell.Execute(cmdrandomhost, false, true);



                // Add new VM
                Command cmdNew = new Command("New-SCVirtualMachine");
                cmdNew.Parameters.Add("Name", vm.Name);
                cmdNew.Parameters.Add("VMTemplate", VMTemplate);
                cmdNew.Parameters.Add("VMHost", vmhost);
                cmdNew.Parameters.Add("Path", vm.RootFolderPath);
                cmdNew.Parameters.Add("ComputerName", vm.Name);
                cmdNew.Parameters.Add("FullName", vm.Name);
                cmdNew.Parameters.Add("OrgName", vm.Name);
                cmdNew.Parameters.Add("RunAsynchronously");
                PowerShell.Execute(cmdNew, false, true);



                // Delete default adapter (MacAddress in not running and newly created VM is 00-00-00-00-00-00)
                NetworkAdapterHelper.Delete(PowerShell, vm.Name, "000000000000");

                // Set VM
                Command cmdSet = new Command("Set-SCVirtualMachine");
                cmdSet.Parameters.Add("Name", vm.Name);
                //cmdSet.Parameters.Add("SmartPagingFilePath", vm.RootFolderPath);
                //cmdSet.Parameters.Add("SnapshotFileLocation", vm.RootFolderPath);
                // startup/shutdown actions
                var autoStartAction = (AutomaticStartAction) AutomaticStartActionSettings;
                var autoStopAction = (AutomaticStopAction) AutomaticStartActionSettings;
                if (autoStartAction != AutomaticStartAction.Undefined)
                {
                    cmdSet.Parameters.Add("AutomaticStartAction", autoStartAction.ToString());
                    cmdSet.Parameters.Add("AutomaticStartDelay", AutomaticStartupDelaySettings);
                }
                if (autoStopAction != AutomaticStopAction.Undefined)
                    cmdSet.Parameters.Add("AutomaticStopAction", autoStopAction.ToString());
                PowerShell.Execute(cmdSet, true);

                // Get created machine Id
                var createdMachine = GetVirtualMachines().FirstOrDefault(m => m.Name == vm.Name);
                if (createdMachine == null)
                    throw new Exception("Can't find created machine");
                vm.VirtualMachineId = createdMachine.VirtualMachineId;

                // Update common settings
                UpdateVirtualMachine(vm);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("CreateVirtualMachine", ex);
                throw;
            }

            return vm;
        }

        public VirtualMachine UpdateVirtualMachine(VirtualMachine vm)
        {
            HostedSolutionLog.LogStart("UpdateVirtualMachine");
            HostedSolutionLog.DebugInfo("Virtual Machine: {0}", vm.VirtualMachineId);

            try
            {
                var realVm = GetVirtualMachineEx(vm.VirtualMachineId);

                DvdDriveHelper.Update(PowerShell, realVm, vm.DvdDriveInstalled); // Dvd should be before bios because bios sets boot order
                BiosHelper.Update(PowerShell, realVm, vm.BootFromCD, vm.NumLockEnabled);
                VirtualMachineHelper.UpdateProcessors(PowerShell, realVm, vm.CpuCores, CpuLimitSettings, CpuReserveSettings, CpuWeightSettings);
                MemoryHelper.Update(PowerShell, realVm, vm.RamSize, vm.DynamicMemory);
                NetworkAdapterHelper.Update(PowerShell, vm);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("UpdateVirtualMachine", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("UpdateVirtualMachine");
           
            return vm;
        }

        public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState)
        {
            HostedSolutionLog.LogStart("ChangeVirtualMachineState");
            var jobResult = new JobResult();

            var vm = GetVirtualMachine(vmId);

            try
            {
                string cmdTxt;
                List<string> paramList = new List<string>();

                switch (newState)
                {
                    case VirtualMachineRequestedState.Start:
                        cmdTxt = "Start-SCVirtualMachine";
                        break;
                    case VirtualMachineRequestedState.Pause:
                        cmdTxt = "Suspend-VM";
                        break;
                    case VirtualMachineRequestedState.Reset:
                        cmdTxt = "Restart-VM";
                        break;
                    case VirtualMachineRequestedState.Resume:
                        cmdTxt = "Resume-VM";
                        break;
                    case VirtualMachineRequestedState.ShutDown:
                        cmdTxt = "Stop-SCVirtualMachine";
                        break;
                    case VirtualMachineRequestedState.TurnOff:
                        cmdTxt = "Stop-VM";
                        paramList.Add("TurnOff");
                        break;
                    case VirtualMachineRequestedState.Save:
                        cmdTxt = "Save-VM";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("newState");
                }

                Command cmd = new Command(cmdTxt);

                cmd.Parameters.Add("VM", vm.Name);
                //cmd.Parameters.Add("AsJob");
                paramList.ForEach(p => cmd.Parameters.Add(p));

                PowerShell.Execute(cmd, true);
                jobResult = JobHelper.CreateSuccessResult(ReturnCode.JobStarted);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("ChangeVirtualMachineState", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("ChangeVirtualMachineState");

            return jobResult;
        }

        public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            var vm = GetVirtualMachine(vmId);

            VirtualMachineHelper.Stop(PowerShell, vm.Name, force, ServerNameSettings);

            return ReturnCode.OK;
        }

        public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            List<ConcreteJob> jobs = new List<ConcreteJob>();

            ManagementBaseObject objSummary = GetVirtualMachineSummaryInformation(
                vmId, SummaryInformationRequest.AsynchronousTasks);
            ManagementBaseObject[] objJobs = (ManagementBaseObject[])objSummary["AsynchronousTasks"];

            if (objJobs != null)
            {
                foreach (ManagementBaseObject objJob in objJobs)
                    jobs.Add(CreateJobFromWmiObject(objJob));
            }

            return jobs;
        }

        public JobResult RenameVirtualMachine(string vmId, string name)
        {
            var vm = GetVirtualMachine(vmId);

            Command cmdSet = new Command("Rename-VM");
            cmdSet.Parameters.Add("Name", vm.Name);
            cmdSet.Parameters.Add("NewName", name);
            PowerShell.Execute(cmdSet, true);

            return JobHelper.CreateSuccessResult();
        }

        public JobResult DeleteVirtualMachine(string vmId)
        {
            var vm = GetVirtualMachineEx(vmId);

            // The virtual computer system must be in the powered off or saved state prior to calling this method.
            if (vm.State != VirtualMachineState.Saved && vm.State != VirtualMachineState.Off)
                throw new Exception("The virtual computer system must be in the powered off or saved state prior to calling Destroy method.");

            // Delete network adapters and network switches
            foreach (var networkAdapter in vm.Adapters)
            {
                NetworkAdapterHelper.Delete(PowerShell, vm.Name, networkAdapter);

                // If more than 1 VM are assigned to the same switch, deleting the virtual machine also deletes the switch which takes other VM instances off line
                // There may be a reason for this that I am not aware of?
                //if (!string.IsNullOrEmpty(networkAdapter.SwitchName))
                    //DeleteSwitch(networkAdapter.SwitchName);
            }

            VirtualMachineHelper.Delete(PowerShell, vm.Name, ServerNameSettings);

            return JobHelper.CreateSuccessResult(ReturnCode.JobStarted);
        }

        public JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            var vm = GetVirtualMachine(vmId);

            // The virtual computer system must be in the powered off or saved state prior to calling this method.
            if (vm.State != VirtualMachineState.Off)
                throw new Exception("The virtual computer system must be in the powered off or saved state prior to calling Export method.");

            Command cmdSet = new Command("Export-VM");
            cmdSet.Parameters.Add("Name", vm.Name);
            cmdSet.Parameters.Add("Path", FileUtils.EvaluateSystemVariables(exportPath));
            PowerShell.Execute(cmdSet, true);
            return JobHelper.CreateSuccessResult(ReturnCode.JobStarted);
        }

        #endregion

        #region Snapshots

        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            List<VirtualMachineSnapshot> snapshots = new List<VirtualMachineSnapshot>();

            try
            {
                var vm = GetVirtualMachine(vmId);

                Command cmd = new Command("Get-VMSnapshot");
                cmd.Parameters.Add("VMName", vm.Name);

                Collection<PSObject> result = PowerShell.Execute(cmd, true);
                if (result != null && result.Count > 0)
                {
                    foreach (PSObject psSnapshot in result)
                    {
                        snapshots.Add(SnapshotHelper.GetFromPS(psSnapshot, vm.ParentSnapshotId));
                    }
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetVirtualMachineSnapshots", ex);
                throw;
            }

            return snapshots;
        }

        public VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            try
            {
                Command cmd = new Command("Get-VMSnapshot");
                cmd.Parameters.Add("Id", snapshotId);

                Collection<PSObject> result = PowerShell.Execute(cmd, true);
                if (result != null && result.Count > 0)
                {
                    return SnapshotHelper.GetFromPS(result[0]);
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetSnapshot", ex);
                throw;
            }

            return null;
        }

        public JobResult CreateSnapshot(string vmId)
        {
            try
            {
                var vm = GetVirtualMachine(vmId);

                Command cmd = new Command("Checkpoint-VM");
                cmd.Parameters.Add("Name", vm.Name);

                PowerShell.Execute(cmd, true);
                return JobHelper.CreateSuccessResult(ReturnCode.JobStarted);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("CreateSnapshot", ex);
                throw;
            }
        }

        public JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            try
            {
                var vm = GetVirtualMachine(vmId);
                var snapshot = GetSnapshot(snapshotId);

                Command cmd = new Command("Rename-VMSnapshot");
                cmd.Parameters.Add("VMName", vm.Name);
                cmd.Parameters.Add("Name", snapshot.Name);
                cmd.Parameters.Add("NewName", name);

                PowerShell.Execute(cmd, true);
                return JobHelper.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("RenameSnapshot", ex);
                throw;
            }
        }

        public JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            try
            {
                var vm = GetVirtualMachine(vmId);
                var snapshot = GetSnapshot(snapshotId);

                Command cmd = new Command("Restore-VMSnapshot");
                cmd.Parameters.Add("VMName", vm.Name);
                cmd.Parameters.Add("Name", snapshot.Name);

                PowerShell.Execute(cmd, true);
                return JobHelper.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("ApplySnapshot", ex);
                throw;
            }
        }

        public JobResult DeleteSnapshot(string snapshotId)
        {
            try
            {
                var snapshot = GetSnapshot(snapshotId);
                SnapshotHelper.Delete(PowerShell, snapshot, false);
                return JobHelper.CreateSuccessResult(ReturnCode.JobStarted);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("DeleteSnapshot", ex);
                throw;
            }
        }

        public JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            try
            {
                var snapshot = GetSnapshot(snapshotId);
                SnapshotHelper.Delete(PowerShell, snapshot, true);
                return JobHelper.CreateSuccessResult(ReturnCode.JobStarted);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("DeleteSnapshot", ex);
                throw;
            }
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            ManagementBaseObject objSummary = GetSnapshotSummaryInformation(snapshotId, (SummaryInformationRequest)size);
            return GetTumbnailFromSummaryInformation(objSummary, size);
        }

        #endregion

        #region DVD operations
        public string GetInsertedDVD(string vmId)
        {
            HostedSolutionLog.LogStart("GetInsertedDVD");
            HostedSolutionLog.DebugInfo("Virtual Machine: {0}", vmId);

            DvdDriveInfo dvdInfo;

            try
            {
                var vm = GetVirtualMachineEx(vmId);
                dvdInfo = DvdDriveHelper.Get(PowerShell, vm.Name);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetInsertedDVD", ex);
                throw;
            }

            if (dvdInfo == null)
                return null;

            HostedSolutionLog.LogEnd("GetInsertedDVD");
            return dvdInfo.Path;
        }

        public JobResult InsertDVD(string vmId, string isoPath)
        {
            HostedSolutionLog.LogStart("InsertDVD");
            HostedSolutionLog.DebugInfo("Virtual Machine: {0}", vmId);
            HostedSolutionLog.DebugInfo("Path: {0}", isoPath);

            try
            {
                var vm = GetVirtualMachineEx(vmId);
                DvdDriveHelper.Set(PowerShell, vm.Name, isoPath);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("InsertDVD", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("InsertDVD");
            return JobHelper.CreateSuccessResult();
        }

        public JobResult EjectDVD(string vmId)
        {
            HostedSolutionLog.LogStart("InsertDVD");
            HostedSolutionLog.DebugInfo("Virtual Machine: {0}", vmId);

            try
            {
                var vm = GetVirtualMachineEx(vmId);
                DvdDriveHelper.Set(PowerShell, vm.Name, null);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("InsertDVD", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("InsertDVD");
            return JobHelper.CreateSuccessResult();
        }
        #endregion

        #region Virtual Switches
        public List<VirtualSwitch> GetSwitches()
        {
            return GetSwitches(null, null);
        }

        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return GetSwitches(computerName, "Public");
        }

        private List<VirtualSwitch> GetSwitches(string computerName, string type)
        {
            HostedSolutionLog.LogStart("GetSwitches");
            HostedSolutionLog.DebugInfo("ComputerName: {0}", computerName);

            List<VirtualSwitch> switches = new List<VirtualSwitch>();

            try
            {
                
                Command cmd = new Command("Get-SCLogicalNetwork");

                // Not needed as the PowerShellManager adds the computer name
                if (!string.IsNullOrEmpty(computerName)) cmd.Parameters.Add("VMMServer", computerName);
                //if (!string.IsNullOrEmpty(type)) cmd.Parameters.Add("Type", type); //VMM doesnt use Types

                Collection<PSObject> result = PowerShell.Execute(cmd, false, true);

                foreach (PSObject current in result)
                {
                    VirtualSwitch sw = new VirtualSwitch();
                    sw.SwitchId = current.GetProperty("Name").ToString();
                    sw.Name = current.GetProperty("Name").ToString();
                    //sw.SwitchType = current.GetProperty("SwitchType").ToString();
                    switches.Add(sw);
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetSwitches", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("GetSwitches");
            return switches;
        }

        public bool SwitchExists(string switchId)
        {
            return GetSwitches().Any(s => s.Name == switchId);
        }

        public VirtualSwitch CreateSwitch(string name)
        {
            // Create private switch

            HostedSolutionLog.LogStart("CreateSwitch");
            HostedSolutionLog.DebugInfo("Name: {0}", name);

            VirtualSwitch virtualSwitch = null;

            try
            {
                Command cmd = new Command("New-VMSwitch");

                cmd.Parameters.Add("SwitchType", "Private");
                cmd.Parameters.Add("Name", name);

                Collection<PSObject> result = PowerShell.Execute(cmd, true);
                if (result != null && result.Count > 0)
                {
                    virtualSwitch = new VirtualSwitch();
                    virtualSwitch.SwitchId = result[0].GetString("Name");
                    virtualSwitch.Name = result[0].GetString("Name");
                    virtualSwitch.SwitchType = result[0].GetString("SwitchType");
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("CreateSwitch", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("CreateSwitch");
            return virtualSwitch;
        }

        public ReturnCode DeleteSwitch(string switchId) // switchId is SwitchName
        {
            HostedSolutionLog.LogStart("DeleteSwitch");
            HostedSolutionLog.DebugInfo("switchId: {0}", switchId);

            try
            {
                Command cmd = new Command("Remove-VMSwitch");
                cmd.Parameters.Add("Name", switchId);
                cmd.Parameters.Add("Force");
                PowerShell.Execute(cmd, true, false);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("DeleteSwitch", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("DeleteSwitch");
            return ReturnCode.OK;
        }
        #endregion

        #region KVP
        public List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            return GetKVPItems(vmId, "GuestExchangeItems");
        }

        public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            return GetKVPItems(vmId, "GuestIntrinsicExchangeItems");
        }

        private List<KvpExchangeDataItem> GetKVPItems(string vmId, string exchangeItemsName)
        {
            List<KvpExchangeDataItem> pairs = new List<KvpExchangeDataItem>();

            // load VM
            ManagementObject objVm = GetVirtualMachineObject(vmId);

            ManagementObject objKvpExchange = null;

            try
            {
                objKvpExchange = wmi.GetRelatedWmiObject(objVm, "msvm_KvpExchangeComponent");
            }
            catch
            {
                HostedSolutionLog.LogError("GetKVPItems", new Exception("msvm_KvpExchangeComponent"));

                return pairs;
            }

            // return XML pairs
            string[] xmlPairs = (string[])objKvpExchange[exchangeItemsName];

            if (xmlPairs == null)
                return pairs;

            // join all pairs
            StringBuilder sb = new StringBuilder();
            sb.Append("<result>");
            foreach (string xmlPair in xmlPairs)
                sb.Append(xmlPair);
            sb.Append("</result>");

            // parse pairs
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());

            foreach (XmlNode nodeName in doc.SelectNodes("/result/INSTANCE/PROPERTY[@NAME='Name']/VALUE"))
            {
                string name = nodeName.InnerText;
                string data = nodeName.ParentNode.ParentNode.SelectSingleNode("PROPERTY[@NAME='Data']/VALUE").InnerText;
                pairs.Add(new KvpExchangeDataItem(name, data));
            }

            return pairs; 
            
            //HostedSolutionLog.LogStart("GetKVPItems");
            //HostedSolutionLog.DebugInfo("Virtual Machine: {0}", vmId);
            //HostedSolutionLog.DebugInfo("exchangeItemsName: {0}", exchangeItemsName);

            //List<KvpExchangeDataItem> pairs = new List<KvpExchangeDataItem>();

            //try
            //{
            //    var vm = GetVirtualMachine(vmId);

            //    Command cmdGetVm = new Command("Get-WmiObject");

            //    cmdGetVm.Parameters.Add("Namespace", WMI_VIRTUALIZATION_NAMESPACE);
            //    cmdGetVm.Parameters.Add("Class", "Msvm_ComputerSystem");
            //    cmdGetVm.Parameters.Add("Filter", "ElementName = '" + vm.Name + "'");

            //    Collection<PSObject> result = PowerShell.Execute(cmdGetVm, false);

            //    if (result != null && result.Count > 0)
            //    {
            //        dynamic resultDynamic = result[0];//.Invoke();
            //        var kvp = resultDynamic.GetRelated("Msvm_KvpExchangeComponent");

            //        // return XML pairs
            //        string[] xmlPairs = null; 
                    
            //        foreach (dynamic a in kvp)
            //        {
            //            xmlPairs = a[exchangeItemsName];
            //            break;
            //        }
                    
            //        if (xmlPairs == null)
            //            return pairs;

            //        // join all pairs
            //        StringBuilder sb = new StringBuilder();
            //        sb.Append("<result>");
            //        foreach (string xmlPair in xmlPairs)
            //            sb.Append(xmlPair);
            //        sb.Append("</result>");

            //        // parse pairs
            //        XmlDocument doc = new XmlDocument();
            //        doc.LoadXml(sb.ToString());

            //        foreach (XmlNode nodeName in doc.SelectNodes("/result/INSTANCE/PROPERTY[@NAME='Name']/VALUE"))
            //        {
            //            string name = nodeName.InnerText;
            //            string data = nodeName.ParentNode.ParentNode.SelectSingleNode("PROPERTY[@NAME='Data']/VALUE").InnerText;
            //            pairs.Add(new KvpExchangeDataItem(name, data));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HostedSolutionLog.LogError("GetKVPItems", ex);
            //    throw;
            //}

            //HostedSolutionLog.LogEnd("GetKVPItems");

            //return pairs; 
        }

        public JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            // get KVP management object
            ManagementObject objVmsvc = GetVirtualSystemManagementService();

            // create KVP items array
            string[] wmiItems = new string[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                ManagementClass clsKvp = wmi.GetWmiClass("Msvm_KvpExchangeDataItem");
                ManagementObject objKvp = clsKvp.CreateInstance();
                objKvp["Name"] = items[i].Name;
                objKvp["Data"] = items[i].Data;
                objKvp["Source"] = 0;

                // convert to WMI format
                wmiItems[i] = objKvp.GetText(TextFormat.CimDtd20);
            }

            ManagementBaseObject inParams = objVmsvc.GetMethodParameters("AddKvpItems");
            inParams["TargetSystem"] = GetVirtualMachineObject(vmId);
            inParams["DataItems"] = wmiItems;

            // invoke method
            ManagementBaseObject outParams = objVmsvc.InvokeMethod("AddKvpItems", inParams, null);
            return CreateJobResultFromWmiMethodResults(outParams);
        }

        public JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            // get KVP management object
            ManagementObject objVmsvc = GetVirtualSystemManagementService();

            // delete items one by one
            for (int i = 0; i < itemNames.Length; i++)
            {
                ManagementClass clsKvp = wmi.GetWmiClass("Msvm_KvpExchangeDataItem");
                ManagementObject objKvp = clsKvp.CreateInstance();
                objKvp["Name"] = itemNames[i];
                objKvp["Data"] = "";
                objKvp["Source"] = 0;

                // convert to WMI format
                string wmiItem = objKvp.GetText(TextFormat.CimDtd20);

                // call method
                ManagementBaseObject inParams = objVmsvc.GetMethodParameters("RemoveKvpItems");
                inParams["TargetSystem"] = GetVirtualMachineObject(vmId);
                inParams["DataItems"] = new string[] { wmiItem };

                // invoke method
                objVmsvc.InvokeMethod("RemoveKvpItems", inParams, null);
            }
            return null;
        }

        public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            // get KVP management object
            ManagementObject objVmsvc = GetVirtualSystemManagementService();

            // create KVP items array
            string[] wmiItems = new string[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                ManagementClass clsKvp = wmi.GetWmiClass("Msvm_KvpExchangeDataItem");
                ManagementObject objKvp = clsKvp.CreateInstance();
                objKvp["Name"] = items[i].Name;
                objKvp["Data"] = items[i].Data;
                objKvp["Source"] = 0;

                // convert to WMI format
                wmiItems[i] = objKvp.GetText(TextFormat.CimDtd20);
            }

            ManagementBaseObject inParams = objVmsvc.GetMethodParameters("ModifyKvpItems");
            inParams["TargetSystem"] = GetVirtualMachineObject(vmId);
            inParams["DataItems"] = wmiItems;

            // invoke method
            ManagementBaseObject outParams = objVmsvc.InvokeMethod("ModifyKvpItems", inParams, null);
            return CreateJobResultFromWmiMethodResults(outParams);
        }
        #endregion

        #region Storage
        public VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            try
            {
                VirtualHardDiskInfo hardDiskInfo = new VirtualHardDiskInfo();
                //HardDriveHelper.GetVirtualHardDiskDetail(PowerShell, vhdPath, ref hardDiskInfo);
                return hardDiskInfo;
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetVirtualHardDiskInfo", ex);
                throw;
            }
        }

        public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            vhdPath = FileUtils.EvaluateSystemVariables(vhdPath);

            // Mount disk
            Command cmd = new Command("Mount-VHD");

            cmd.Parameters.Add("Path", vhdPath);
            cmd.Parameters.Add("PassThru");

            // Get mounted disk 
            var result = PowerShell.Execute(cmd, true, true);

            try
            {
                if (result == null || result.Count == 0)
                    throw new Exception("Failed to mount disk");

                var diskNumber = result[0].GetInt("DiskNumber");

                var diskInfo = VdsHelper.GetMountedDiskInfo(ServerNameSettings, diskNumber);
                
                return diskInfo;
            }
            catch (Exception ex)
            {
                // unmount disk
                UnmountVirtualHardDisk(vhdPath);

                // throw error
                throw ex;
            }
        }

        public ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            Command cmd = new Command("Dismount-VHD");

            cmd.Parameters.Add("Path", FileUtils.EvaluateSystemVariables(vhdPath));

            PowerShell.Execute(cmd, true);
            return ReturnCode.OK;
        }

        public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            Command cmd = new Command("Resize-VHD");

            cmd.Parameters.Add("Path", FileUtils.EvaluateSystemVariables(vhdPath));
            cmd.Parameters.Add("SizeBytes", sizeGB * Constants.Size1G);

            PowerShell.Execute(cmd, true, true);
            return JobHelper.CreateSuccessResult(ReturnCode.JobStarted); 
        }

        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            return JobHelper.CreateSuccessResult(ReturnCode.JobStarted);
        }

        public void DeleteRemoteFile(string path)
        {
            if (DirectoryExists(path))
                DeleteFolder(path); // WMI way
            else if (FileExists(path))
                DeleteFile(path); // WMI way
        }

        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            // find mounted disk using VDS
            Vds.Advanced.AdvancedDisk advancedDisk = null;
            Vds.Pack diskPack = null;

            VdsHelper.FindVdsDisk(ServerNameSettings, diskAddress, out advancedDisk, out diskPack);

            if (advancedDisk == null)
                throw new Exception("Could not find mounted disk");

            // find volume
            Vds.Volume diskVolume = null;
            foreach (Vds.Volume volume in diskPack.Volumes)
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
            foreach (Vds.DiskExtent extent in advancedDisk.Extents)
            {
                if (extent.Type != Microsoft.Storage.Vds.DiskExtentType.Free)
                    continue;

                if (extent.Size > oneMegabyte)
                    freeSpace += extent.Size;
            }

            if (freeSpace == 0)
                return;

            // input disk
            Vds.InputDisk inputDisk = new Vds.InputDisk();
            foreach (Vds.VolumePlex plex in diskVolume.Plexes)
            {
                inputDisk.DiskId = advancedDisk.Id;
                inputDisk.Size = freeSpace;
                inputDisk.PlexId = plex.Id;

                foreach (Vds.DiskExtent extent in plex.Extents)
                    inputDisk.MemberIndex = extent.MemberIndex;

                break;
            }

            // extend volume
            Vds.Async extendEvent = diskVolume.BeginExtend(new Vds.InputDisk[] { inputDisk }, null, null);
            while (!extendEvent.IsCompleted)
                System.Threading.Thread.Sleep(100);
            diskVolume.EndExtend(extendEvent);
        }

       
        public string ReadRemoteFile(string path)
        {
            // temp file name on "system" drive available through hidden share
            string tempPath = Path.Combine(VdsHelper.GetTempRemoteFolder(ServerNameSettings), Guid.NewGuid().ToString("N"));

            HostedSolutionLog.LogInfo("Read remote file: " + path);
            HostedSolutionLog.LogInfo("Local file temp path: " + tempPath);

            // copy remote file to temp file (WMI)
            if (!CopyFile(path, tempPath))
                return null;

            // read content of temp file
            string remoteTempPath = VdsHelper.ConvertToUNC(ServerNameSettings, tempPath);
            HostedSolutionLog.LogInfo("Remote file temp path: " + remoteTempPath);

            string content = File.ReadAllText(remoteTempPath);

            // delete temp file (WMI)
            DeleteFile(tempPath);

            return content;
        }

        public void WriteRemoteFile(string path, string content)
        {
            // temp file name on "system" drive available through hidden share
            string tempPath = Path.Combine(VdsHelper.GetTempRemoteFolder(ServerNameSettings), Guid.NewGuid().ToString("N"));

            // write to temp file
            string remoteTempPath = VdsHelper.ConvertToUNC(ServerNameSettings, tempPath);
            File.WriteAllText(remoteTempPath, content);

            // delete file (WMI)
            if (FileExists(path))
                DeleteFile(path);

            // copy (WMI)
            CopyFile(tempPath, path);

            // delete temp file (WMI)
            DeleteFile(tempPath);
        }
        #endregion

        #region Jobs
        public ConcreteJob GetJob(string jobId)
        {
            throw new NotImplementedException();

            //HostedSolutionLog.LogStart("GetJob");
            //HostedSolutionLog.DebugInfo("jobId: {0}", jobId);

            //Runspace runSpace = null;
            //ConcreteJob job;

            //try
            //{
            //    Command cmd = new Command("Get-Job");

            //    if (!string.IsNullOrEmpty(jobId)) cmd.Parameters.Add("Id", jobId);

            //    Collection<PSObject> result = PowerShell.Execute(cmd, true);
            //    job = JobHelper.CreateFromPSObject(result);
            //}
            //catch (Exception ex)
            //{
            //    HostedSolutionLog.LogError("GetJob", ex);
            //    throw;
            //}

            //HostedSolutionLog.LogEnd("GetJob");
            //return job;
        }

        public List<ConcreteJob> GetAllJobs()
        {
            throw new NotImplementedException();
        }

        public ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Configuration
        public int GetProcessorCoresNumber()
        {
            Wmi w = new Wmi(ServerNameSettings, @"root\cimv2");
            ManagementObject objCpu = w.GetWmiObject("win32_Processor");
            return Convert.ToInt32(objCpu["NumberOfCores"]);
        }
        #endregion

        #region IHostingServiceProvier methods
        public override string[] Install()
        {
            List<string> messages = new List<string>();

            // TODO

            return messages.ToArray();
        }

        public override bool IsInstalled()
        {
            // check if Hyper-V role is installed and available for management
            //Wmi root = new Wmi(ServerNameSettings, "root");
            //ManagementObject objNamespace = root.GetWmiObject("__NAMESPACE", "name = 'virtualization'");
            //return (objNamespace != null);
            return true;
        }

        public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
        {
            foreach (ServiceProviderItem item in items)
            {
                if (item is VirtualMachine)
                {
                    // start/stop virtual machine
                    VirtualMachine vm = item as VirtualMachine;
                    ChangeVirtualMachineServiceItemState(vm, enabled);
                }
            }
        }

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                if (item is VirtualMachine)
                {
                    // delete virtual machine
                    VirtualMachine vm = item as VirtualMachine;
                    DeleteVirtualMachineServiceItem(vm);
                }
                else if (item is VirtualSwitch)
                {
                    // delete switch
                    VirtualSwitch vs = item as VirtualSwitch;
                    DeleteVirtualSwitchServiceItem(vs);
                }
            }
        }

        private void ChangeVirtualMachineServiceItemState(VirtualMachine vm, bool started)
        {
            try
            {
                VirtualMachine vps = GetVirtualMachine(vm.VirtualMachineId);
                JobResult result = null;

                if (vps == null)
                {
                    HostedSolutionLog.LogWarning(String.Format("Virtual machine '{0}' object with ID '{1}' was not found. Change state operation aborted.",
                        vm.Name, vm.VirtualMachineId));
                    return;
                }

                #region Start
                if (started &&
                    (vps.State == VirtualMachineState.Off
                    || vps.State == VirtualMachineState.Paused
                    || vps.State == VirtualMachineState.Saved))
                {
                    VirtualMachineRequestedState state = VirtualMachineRequestedState.Start;
                    if (vps.State == VirtualMachineState.Paused)
                        state = VirtualMachineRequestedState.Resume;

                    result = ChangeVirtualMachineState(vm.VirtualMachineId, state);

                    // check result
                    if (result.ReturnValue != ReturnCode.JobStarted)
                    {
                        HostedSolutionLog.LogWarning(String.Format("Cannot {0} '{1}' virtual machine: {2}",
                            state, vm.Name, result.ReturnValue));
                        return;
                    }

                    // wait for completion
                    if (!JobCompleted(result.Job))
                    {
                        HostedSolutionLog.LogWarning(String.Format("Cannot complete {0} '{1}' of virtual machine: {1}",
                            state, vm.Name, result.Job.ErrorDescription));
                        return;
                    }
                }
                #endregion

                #region Stop
                else if (!started &&
                    (vps.State == VirtualMachineState.Running
                    || vps.State == VirtualMachineState.Paused))
                {
                    if (vps.State == VirtualMachineState.Running)
                    {
                        // try to shutdown the system
                        ReturnCode code = ShutDownVirtualMachine(vm.VirtualMachineId, true, "Virtual Machine has been suspended from SolidCP");
                        if (code == ReturnCode.OK)
                            return;
                    }

                    // turn off
                    VirtualMachineRequestedState state = VirtualMachineRequestedState.TurnOff;
                    result = ChangeVirtualMachineState(vm.VirtualMachineId, state);

                    // check result
                    if (result.ReturnValue != ReturnCode.JobStarted)
                    {
                        HostedSolutionLog.LogWarning(String.Format("Cannot {0} '{1}' virtual machine: {2}",
                            state, vm.Name, result.ReturnValue));
                        return;
                    }

                    // wait for completion
                    if (!JobCompleted(result.Job))
                    {
                        HostedSolutionLog.LogWarning(String.Format("Cannot complete {0} '{1}' of virtual machine: {1}",
                            state, vm.Name, result.Job.ErrorDescription));
                        return;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(String.Format("Error {0} Virtual Machine '{1}'",
                    started ? "starting" : "turning off",
                    vm.Name), ex);
            }
        }

        private void DeleteVirtualMachineServiceItem(VirtualMachine vm)
        {
            try
            {
                JobResult result = null;
                VirtualMachine vps = GetVirtualMachine(vm.VirtualMachineId);

                if (vps == null)
                {
                    HostedSolutionLog.LogWarning(String.Format("Virtual machine '{0}' object with ID '{1}' was not found. Delete operation aborted.",
                        vm.Name, vm.VirtualMachineId));
                    return;
                }

                #region Turn off (if required)
                if (vps.State != VirtualMachineState.Off)
                {
                    result = ChangeVirtualMachineState(vm.VirtualMachineId, VirtualMachineRequestedState.TurnOff);
                    // check result
                    if (result.ReturnValue != ReturnCode.JobStarted)
                    {
                        HostedSolutionLog.LogWarning(String.Format("Cannot Turn off '{0}' virtual machine before deletion: {1}",
                            vm.Name, result.ReturnValue));
                        return;
                    }

                    // wait for completion
                    if (!JobCompleted(result.Job))
                    {
                        HostedSolutionLog.LogWarning(String.Format("Cannot complete Turn off '{0}' of virtual machine before deletion: {1}",
                            vm.Name, result.Job.ErrorDescription));
                        return;
                    }
                }
                #endregion

                #region Delete virtual machine
                result = DeleteVirtualMachine(vm.VirtualMachineId);

                // check result
                if (result.ReturnValue != ReturnCode.JobStarted)
                {
                    HostedSolutionLog.LogWarning(String.Format("Cannot delete '{0}' virtual machine: {1}",
                        vm.Name, result.ReturnValue));
                    return;
                }

                // wait for completion
                if (!JobCompleted(result.Job))
                {
                    HostedSolutionLog.LogWarning(String.Format("Cannot complete deletion of '{0}' virtual machine: {1}",
                        vm.Name, result.Job.ErrorDescription));
                    return;
                }
                #endregion

                #region Delete virtual machine
                try
                {
                    DeleteFile(vm.RootFolderPath);
                }
                catch (Exception ex)
                {
                    HostedSolutionLog.LogError(String.Format("Cannot delete virtual machine folder '{0}'",
                        vm.RootFolderPath), ex);
                }
                #endregion

            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(String.Format("Error deleting Virtual Machine '{0}'", vm.Name), ex);
            }
        }

        private void DeleteVirtualSwitchServiceItem(VirtualSwitch vs)
        {
            try
            {
                // delete virtual switch
                DeleteSwitch(vs.SwitchId);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(String.Format("Error deleting Virtual Switch '{0}'", vs.Name), ex);
            }
        }
        #endregion

        #region Private Methods

        internal int ConvertNullableToInt32(object value)
        {
            return value == null ? 0 : Convert.ToInt32(value);
        }

        internal long ConvertNullableToInt64(object value)
        {
            return value == null ? 0 : Convert.ToInt64(value);
        }

        protected JobResult CreateJobResultFromWmiMethodResults(ManagementBaseObject outParams)
        {
            JobResult result = new JobResult();

            // return value
            result.ReturnValue = (ReturnCode)Convert.ToInt32(outParams["ReturnValue"]);

            // try getting job details job
            try
            {
                ManagementBaseObject objJob = wmi.GetWmiObjectByPath((string)outParams["Job"]);
                if (objJob != null && objJob.Properties.Count > 0)
                {
                    result.Job = CreateJobFromWmiObject(objJob);
                }
            }
            catch { /* dumb */ }

            return result;
        }

        private ManagementObject GetVirtualSystemManagementService()
        {
            return wmi.GetWmiObject("msvm_VirtualSystemManagementService");
        }

        protected ManagementObject GetImageManagementService()
        {
            return wmi.GetWmiObject("msvm_ImageManagementService");
        }

        private ManagementObject GetVirtualMachineObject(string vmId)
        {
            return wmi.GetWmiObject("msvm_ComputerSystem", "Name = '{0}'", vmId);
        }

        private ManagementObject GetSnapshotObject(string snapshotId)
        {
            return wmi.GetWmiObject("Msvm_VirtualSystemSettingData", "InstanceID = '{0}'", snapshotId) ??
                   wmi.GetWmiObject("Msvm_VirtualSystemSettingData", "InstanceID = '{0}'", "Microsoft:" + snapshotId);
        }


        private ConcreteJob CreateJobFromWmiObject(ManagementBaseObject objJob)
        {
            if (objJob == null || objJob.Properties.Count == 0)
                return null;

            ConcreteJob job = new ConcreteJob();
            job.Id = (string)objJob["InstanceID"];
            job.JobState = (ConcreteJobState)Convert.ToInt32(objJob["JobState"]);
            job.Caption = (string)objJob["Caption"];
            job.Description = (string)objJob["Description"];
            job.StartTime = Wmi.ToDateTime((string)objJob["StartTime"]);
            // TODO proper parsing of WMI time spans, e.g. 00000000000001.325247:000
            job.ElapsedTime = DateTime.Now; //wmi.ToDateTime((string)objJob["ElapsedTime"]);
            job.ErrorCode = Convert.ToInt32(objJob["ErrorCode"]);
            job.ErrorDescription = (string)objJob["ErrorDescription"];
            job.PercentComplete = Convert.ToInt32(objJob["PercentComplete"]);
            return job;
        }

        private ManagementBaseObject GetSnapshotSummaryInformation(
            string snapshotId,
            SummaryInformationRequest requestedInformation)
        {
            // find VM settings object
            ManagementObject objVmSetting = GetSnapshotObject(snapshotId);

            // get summary
            return GetSummaryInformation(objVmSetting, requestedInformation);
        }

        private ManagementBaseObject GetVirtualMachineSummaryInformation(
            string vmId,
            params SummaryInformationRequest[] requestedInformation)
        {
            // find VM settings object
            ManagementObject objVmSetting = GetVirtualMachineSettingsObject(vmId);

            // get summary
            return GetSummaryInformation(objVmSetting, requestedInformation);
        }

        private ManagementBaseObject GetSummaryInformation(
            ManagementObject objVmSetting, params SummaryInformationRequest[] requestedInformation)
        {
            if (requestedInformation == null || requestedInformation.Length == 0)
                throw new ArgumentNullException("requestedInformation");

            // get management service
            ManagementObject objVmsvc = GetVirtualSystemManagementService();

            uint[] reqif = new uint[requestedInformation.Length];
            for (int i = 0; i < requestedInformation.Length; i++)
                reqif[i] = (uint)requestedInformation[i];

            // get method params
            ManagementBaseObject inParams = objVmsvc.GetMethodParameters("GetSummaryInformation");
            inParams["SettingData"] = new ManagementObject[] { objVmSetting };
            inParams["RequestedInformation"] = reqif;

            // invoke method
            ManagementBaseObject outParams = objVmsvc.InvokeMethod("GetSummaryInformation", inParams, null);
            return ((ManagementBaseObject[])outParams["SummaryInformation"])[0];
        }

        private ManagementObject GetVirtualMachineSettingsObject(string vmId)
        {
            return wmi.GetWmiObject("msvm_VirtualSystemSettingData", "InstanceID Like 'Microsoft:{0}%'", vmId);
        }

        private bool JobCompleted(ConcreteJob job)
        {
            bool jobCompleted = true;

            while (job.JobState == ConcreteJobState.Starting ||
                job.JobState == ConcreteJobState.Running)
            {
                System.Threading.Thread.Sleep(200);
                job = GetJob(job.Id);
            }

            if (job.JobState != ConcreteJobState.Completed)
            {
                jobCompleted = false;
            }

            return jobCompleted;
        }
        private void SetUsagesFromKVP(ref VirtualMachine vm)
        {
            // Use the SolidCP VMConfig Windows service to get the RAM usage as well as the HDD usage / sizes
            List<KvpExchangeDataItem> vmKvps = GetKVPItems(vm.VirtualMachineId);
            foreach (KvpExchangeDataItem vmKvp in vmKvps)
            {
                // RAM
                if (vmKvp.Name == Constants.KVP_RAM_SUMMARY_KEY)
                {
                    string[] ram = vmKvp.Data.Split(':');
                    int freeRam = Int32.Parse(ram[0]);
                    int availRam = Int32.Parse(ram[1]);

                    vm.RamUsage = availRam - freeRam;
                }

                // HDD
                if (vmKvp.Name == Constants.KVP_HDD_SUMMARY_KEY)
                {
                    string[] disksArray = vmKvp.Data.Split(';');
                    vm.HddLogicalDisks = new LogicalDisk[disksArray.Length];
                    for (int i = 0; i < disksArray.Length; i++)
                    {
                        string[] disk = disksArray[i].Split(':');
                        vm.HddLogicalDisks[i] = new LogicalDisk
                        {
                            DriveLetter = disk[0],
                            FreeSpace = Int32.Parse(disk[1]),
                            Size = Int32.Parse(disk[2])
                        };
                    }
                }
            }
        }
        #endregion

        #region Remote File Methods
        public bool FileExists(string path)
        {
            HostedSolutionLog.LogInfo("Check remote file exists: " + path);

            if (path.StartsWith(@"\\")) // network share
                return File.Exists(path);
            else
            {
                Wmi cimv2 = new Wmi(ServerNameSettings, Constants.WMI_CIMV2_NAMESPACE);
                ManagementObject objFile = cimv2.GetWmiObject("CIM_Datafile", "Name='{0}'", path.Replace("\\", "\\\\"));
                return (objFile != null);
            }
        }

        public bool DirectoryExists(string path)
        {
            if (path.StartsWith(@"\\")) // network share
                return Directory.Exists(path);
            else
            {
                Wmi cimv2 = new Wmi(ServerNameSettings, Constants.WMI_CIMV2_NAMESPACE);
                ManagementObject objDir = cimv2.GetWmiObject("Win32_Directory", "Name='{0}'", path.Replace("\\", "\\\\"));
                return (objDir != null);
            }
        }

        public bool CopyFile(string sourceFileName, string destinationFileName)
        {
            HostedSolutionLog.LogInfo("Copy file - source: " + sourceFileName);
            HostedSolutionLog.LogInfo("Copy file - destination: " + destinationFileName);

            if (sourceFileName.StartsWith(@"\\")) // network share
            {
                if (!File.Exists(sourceFileName))
                    return false;

                File.Copy(sourceFileName, destinationFileName);
            }
            else
            {
                if (!FileExists(sourceFileName))
                    return false;

                // copy using WMI
                Wmi cimv2 = new Wmi(ServerNameSettings, Constants.WMI_CIMV2_NAMESPACE);
                ManagementObject objFile = cimv2.GetWmiObject("CIM_Datafile", "Name='{0}'", sourceFileName.Replace("\\", "\\\\"));
                if (objFile == null)
                    throw new Exception("Source file does not exists: " + sourceFileName);

                objFile.InvokeMethod("Copy", new object[] { destinationFileName });
            }
            return true;
        }

        public void DeleteFile(string path)
        {
            if (path.StartsWith(@"\\"))
            {
                // network share
                File.Delete(path);
            }
            else
            {
                // delete file using WMI
                Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");
                ManagementObject objFile = cimv2.GetWmiObject("CIM_Datafile", "Name='{0}'", path.Replace("\\", "\\\\"));
                objFile.InvokeMethod("Delete", null);
            }
        }

        public void DeleteFolder(string path)
        {
            if (path.StartsWith(@"\\"))
            {
                // network share
                try
                {
                    FileUtils.DeleteFile(path);
                }
                catch { /* just skip */ }
                FileUtils.DeleteFile(path);
            }
            else
            {
                // local folder
                // delete sub folders first
                ManagementObjectCollection objSubFolders = GetSubFolders(path);
                foreach (ManagementObject objSubFolder in objSubFolders)
                    DeleteFolder(objSubFolder["Name"].ToString());

                // delete this folder itself
                Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");
                ManagementObject objFolder = cimv2.GetWmiObject("Win32_Directory", "Name='{0}'", path.Replace("\\", "\\\\"));
                objFolder.InvokeMethod("Delete", null);
            }
        }

        private ManagementObjectCollection GetSubFolders(string path)
        {
            if (path.EndsWith("\\"))
                path = path.Substring(0, path.Length - 1);

            Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");

            return cimv2.ExecuteWmiQuery("Associators of {Win32_Directory.Name='"
                + path + "'} "
                + "Where AssocClass = Win32_Subdirectory "
                + "ResultRole = PartComponent");
        }

        public void CreateFolder(string path)
        {
            VdsHelper.ExecuteRemoteProcess(ServerNameSettings, String.Format("cmd.exe /c md \"{0}\"", path));
        }


        #endregion

        #region Hyper-V Cloud
        public bool CheckServerState(string connString)
        {
            return !String.IsNullOrEmpty(connString);
        }
        #endregion Hyper-V Cloud

        #region Replication

        public List<CertificateInfo> GetCertificates(string remoteServer)
        {
            // we cant get certificates from remote server
            if (!string.IsNullOrEmpty(remoteServer))
                return null;

            Command cmd = new Command("Get-ChildItem");
            cmd.Parameters.Add("Path", @"cert:\LocalMachine\My");

            Collection<PSObject> result = PowerShell.Execute(cmd, false);

            return result
                .Select(
                    cert => new CertificateInfo
                    {
                        Subject = cert.GetString("Subject"),
                        Thumbprint = cert.GetString("Thumbprint"),
                        Title = string.Format("{0} ({1})", cert.GetString("Thumbprint"), cert.GetString("Subject")),
                    })
                .ToList();
        }

        public void SetReplicaServer(string remoteServer, string thumbprint, string storagePath)
        {
            // we cant enable firewall rules on remote server
            if (string.IsNullOrEmpty(remoteServer)) 
                ReplicaHelper.SetFirewallRule(PowerShell, true);

            if (GetReplicaServer(remoteServer) != null)
                UnsetReplicaServer(remoteServer);

            ReplicaHelper.SetReplicaServer(PowerShell, true, remoteServer, thumbprint, storagePath);
        }

        public void UnsetReplicaServer(string remoteServer)
        {
            ReplicaHelper.SetReplicaServer(PowerShell, false, remoteServer, null, null);
        }

        public ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            ReplicationServerInfo replicaServer = null;
            Command cmd = new Command("Get-VMReplicationServer");

            if (!string.IsNullOrEmpty(remoteServer))
            {
                cmd.Parameters.Add("ComputerName", remoteServer);
            }

            Collection<PSObject> result = PowerShell.Execute(cmd, false);

            if (result != null && result.Count > 0)
            {
                replicaServer = new ReplicationServerInfo();
                replicaServer.Enabled = result[0].GetBool("RepEnabled");
                replicaServer.ComputerName = result[0].GetString("ComputerName");
            }

            return replicaServer;
        }

        public void EnableVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            if (ReplicaMode != ReplicaMode.ReplicationEnabled)
                throw new Exception("Server does not allow replication by settings");

            var vm = GetVirtualMachineEx(vmId);

            Command cmd = new Command("Enable-VMReplication");
            cmd.Parameters.Add("VmName", vm.Name);
            cmd.Parameters.Add("ReplicaServerName", replicaServer);
            cmd.Parameters.Add("ReplicaServerPort", 443);
            cmd.Parameters.Add("AuthenticationType", "Cert");
            cmd.Parameters.Add("CertificateThumbprint", replication.Thumbprint);
            cmd.Parameters.Add("ReplicationFrequencySec", (int)replication.ReplicaFrequency);

            var excludes = vm.Disks
                .Select(d => d.Path)
                .Where(p => replication.VhdToReplicate.All(vp => !p.Equals(vp, StringComparison.OrdinalIgnoreCase)))
                .ToArray();
            if (excludes.Any())
                cmd.Parameters.Add("ExcludedVhdPath", excludes);

            // recovery points
            cmd.Parameters.Add("RecoveryHistory", replication.AdditionalRecoveryPoints);
            if (replication.AdditionalRecoveryPoints > 0)
            {
                if (replication.AdditionalRecoveryPoints > 24)
                    throw new Exception("AdditionalRecoveryPoints can not be greater than 24");

                if (replication.VSSSnapshotFrequencyHour > 0)
                {
                    if (replication.VSSSnapshotFrequencyHour > 12)
                        throw new Exception("VSSSnapshotFrequencyHour can not be greater than 12");

                    cmd.Parameters.Add("VSSSnapshotFrequencyHour", replication.VSSSnapshotFrequencyHour);
                }
            }

            PowerShell.Execute(cmd, true, true);
        }

        public void SetVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            if (ReplicaMode != ReplicaMode.ReplicationEnabled)
                throw new Exception("Server does not allow replication by settings");

            var vm = GetVirtualMachineEx(vmId);

            Command cmd = new Command("Set-VMReplication");
            cmd.Parameters.Add("VmName", vm.Name);
            cmd.Parameters.Add("ReplicaServerName", replicaServer);
            cmd.Parameters.Add("ReplicaServerPort", 443);
            cmd.Parameters.Add("AuthenticationType", "Cert");
            cmd.Parameters.Add("CertificateThumbprint", replication.Thumbprint);
            cmd.Parameters.Add("ReplicationFrequencySec", (int)replication.ReplicaFrequency);

            // recovery points
            cmd.Parameters.Add("RecoveryHistory", replication.AdditionalRecoveryPoints);
            if (replication.AdditionalRecoveryPoints > 0)
            {
                if (replication.AdditionalRecoveryPoints > 24)
                    throw new Exception("AdditionalRecoveryPoints can not be greater than 24");

                if (replication.VSSSnapshotFrequencyHour > 0)
                {
                    if (replication.VSSSnapshotFrequencyHour > 12)
                        throw new Exception("VSSSnapshotFrequencyHour can not be greater than 12");

                    cmd.Parameters.Add("VSSSnapshotFrequencyHour", replication.VSSSnapshotFrequencyHour);
                }
                else
                {
                    cmd.Parameters.Add("DisableVSSSnapshotReplication");
                }
            }

            PowerShell.Execute(cmd, true, true);
        }

        public void TestReplicationServer(string vmId, string replicaServer, string localThumbprint)
        {
            if (ReplicaMode != ReplicaMode.ReplicationEnabled)
                throw new Exception("Server does not allow replication by settings");

            var vm = GetVirtualMachine(vmId);

            Command cmd = new Command("Test-VMReplicationConnection");
            cmd.Parameters.Add("VmName", vm.Name);
            cmd.Parameters.Add("ReplicaServerName", replicaServer);
            cmd.Parameters.Add("ReplicaServerPort", 443);
            cmd.Parameters.Add("AuthenticationType", "Cert");
            cmd.Parameters.Add("CertificateThumbprint", localThumbprint);

            PowerShell.Execute(cmd, true);
        }

        public void StartInitialReplication(string vmId)
        {
            if (ReplicaMode != ReplicaMode.ReplicationEnabled)
                throw new Exception("Server does not allow replication by settings");

            var vm = GetVirtualMachine(vmId);

            Command cmd = new Command("Start-VMInitialReplication");
            cmd.Parameters.Add("VmName", vm.Name);

            PowerShell.Execute(cmd, true);
        }

        public VmReplication GetReplication(string vmId)
        {
            if (ReplicaMode != ReplicaMode.ReplicationEnabled)
                throw new Exception("Server does not allow replication by settings");

            VmReplication replica = null;
            var vm = GetVirtualMachineEx(vmId);

            Command cmd = new Command("Get-VMReplication");
            cmd.Parameters.Add("VmName", vm.Name);

            Collection<PSObject> result = PowerShell.Execute(cmd, true);

            if (result != null && result.Count > 0)
            {
                replica = new VmReplication();
                replica.ReplicaFrequency = result[0].GetEnum<ReplicaFrequency>("FrequencySec", ReplicaFrequency.Seconds30);
                replica.Thumbprint = result[0].GetString("CertificateThumbprint");
                replica.AdditionalRecoveryPoints = result[0].GetInt("RecoveryHistory");
                replica.VSSSnapshotFrequencyHour = result[0].GetInt("VSSSnapshotFrequencyHour");

                List<string> excludes = new List<string>();
                foreach (dynamic item in (IEnumerable) result[0].GetProperty("ExcludedDisks"))
                    excludes.Add(item.Path.ToString());
                replica.VhdToReplicate = vm.Disks
                    .Select(d => d.Path)
                    .Where(p => excludes.All(ep => !p.Equals(ep, StringComparison.OrdinalIgnoreCase)))
                    .ToArray();
            }

            return replica;
        }

        public void DisableVmReplication(string vmId)
        {
            if (ReplicaMode == ReplicaMode.None)
                throw new Exception("Server does not allow replication by settings");

            var vm = GetVirtualMachine(vmId);

            ReplicaHelper.RemoveVmReplication(PowerShell, vm.Name, ServerNameSettings);
        }


        public ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            if (ReplicaMode != ReplicaMode.ReplicationEnabled)
                throw new Exception("Server does not allow replication by settings");

            ReplicationDetailInfo replica = null;
            var vm = GetVirtualMachine(vmId);

            Command cmd = new Command("Measure-VMReplication");
            cmd.Parameters.Add("VmName", vm.Name);

            Collection<PSObject> result = PowerShell.Execute(cmd, true);

            if (result != null && result.Count > 0)
            {
                replica = new ReplicationDetailInfo();
                replica.AverageLatency = result[0].GetProperty<TimeSpan?>("AverageReplicationLatency") ?? new TimeSpan();
                replica.AverageSize = result[0].GetMb("AverageReplicationSize");
                replica.Errors = result[0].GetInt("ReplicationErrors");
                replica.FromTime = result[0].GetProperty<DateTime>("MonitoringStartTime");
                replica.Health = result[0].GetEnum<ReplicationHealth>("ReplicationHealth");
                replica.HealthDetails = string.Join(" ", result[0].GetProperty<string[]>("ReplicationHealthDetails"));
                replica.LastSynhronizedAt = result[0].GetProperty<DateTime?>("LastReplicationTime") ?? new DateTime();
                replica.MaximumSize = result[0].GetMb("MaximumReplicationSize");
                replica.Mode = result[0].GetEnum<VmReplicationMode>("ReplicationMode");
                replica.PendingSize = result[0].GetMb("PendingReplicationSize");
                replica.PrimaryServerName = result[0].GetString("PrimaryServerName");
                replica.ReplicaServerName = result[0].GetString("CurrentReplicaServerName");
                replica.State = result[0].GetEnum<ReplicationState>("ReplicationState");
                replica.SuccessfulReplications = result[0].GetInt("SuccessfulReplicationCount");
                replica.MissedReplicationCount = result[0].GetInt("MissedReplicationCount");
                replica.ToTime = result[0].GetProperty<DateTime>("MonitoringEndTime");
            }

            return replica;
        }

        public void PauseReplication(string vmId)
        {
            if (ReplicaMode != ReplicaMode.ReplicationEnabled)
                throw new Exception("Server does not allow replication by settings");

            var vm = GetVirtualMachine(vmId);

            Command cmd = new Command("Suspend-VMReplication");
            cmd.Parameters.Add("VmName", vm.Name);

            PowerShell.Execute(cmd, true);
        }

        public void ResumeReplication(string vmId)
        {
            if (ReplicaMode != ReplicaMode.ReplicationEnabled)
                throw new Exception("Server does not allow replication by settings");

            var vm = GetVirtualMachine(vmId);

            Command cmd = new Command("Resume-VMReplication");
            cmd.Parameters.Add("VmName", vm.Name);

            PowerShell.Execute(cmd, true);
        }
        #endregion
    }
}