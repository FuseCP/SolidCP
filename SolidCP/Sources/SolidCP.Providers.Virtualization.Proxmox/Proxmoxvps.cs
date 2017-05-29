// Copyright (c) 2017, Centron GmbH
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2014, Outercurve Foundation.
// All rights reserved.
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
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
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

using RestSharp;
using System.Net;
using System.Dynamic;
using System.Threading;

using System.Xml;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

using System.Configuration;
﻿using System.Linq;
﻿using SolidCP.Providers.Virtualization.Extensions;
using SolidCP.Providers.Virtualization.Proxmox;

using Renci.SshNet;


namespace SolidCP.Providers.Virtualization
{
    public class Proxmoxvps : HostingServiceProviderBase, IVirtualizationServerProxmox
    {

        #region Provider Settings
        protected string ProxmoxClusterServerHost
        {
            get { return ProviderSettings["ProxmoxClusterServerHost"]; }
        }

        protected string ProxmoxClusterServerPort
        {
            get { return ProviderSettings["ProxmoxClusterServerPort"]; }
        }

        protected string ProxmoxClusterAdminUser
        {
            get { return ProviderSettings["ProxmoxClusterAdminUser"]; }
        }

        protected string ProxmoxClusterRealm
        {
            get { return ProviderSettings["ProxmoxClusterRealm"]; }
        }

        protected string ProxmoxClusterAdminPass
        {
            get { return ProviderSettings["ProxmoxClusterAdminPass"]; }
        }

        /*
        protected string ProxmoxClusterNode
        {
            get { return ProviderSettings["ProxmoxClusterNode"]; }
        }
        */

        protected string ProxmoxIsosonStorage
        {
            get { return ProviderSettings["ProxmoxIsosonStorage"]; }
        }

        protected string ServerNameSettings
        {
            get { return ProviderSettings["ServerName"]; }
        }

        protected string DeploySSHServerHostSettings
        {
            get { return ProviderSettings["DeploySSHServerHost"]; }
        }

        protected string DeploySSHServerPortSettings
        {
            get { return ProviderSettings["DeploySSHServerPort"]; }
        }

        protected string DeploySSHUserSettings
        {
            get { return ProviderSettings["DeploySSHUser"]; }
        }

        protected string DeploySSHPassSettings
        {
            get { return ProviderSettings["DeploySSHPass"]; }
        }

        protected string DeploySSHKeySettings
        {
            get { return ProviderSettings["DeploySSHKey"]; }
        }

        protected string DeploySSHKeyPassSettings
        {
            get { return ProviderSettings["DeploySSHKeyPass"]; }
        }

        protected string DeploySSHScriptSettings
        {
            get { return ProviderSettings["DeploySSHScript"]; }
        }

        protected string DeploySSHScriptParamsSettings
        {
            get { return ProviderSettings["DeploySSHScriptParams"]; }
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
            get { return 1; }
        }

        /*
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
        */
        #endregion

        #region Fields
        // proxmox
        private const string THUMB_PRINT_TEXT = "PROXMOX VPS";
        private const string SNAPSHOT_DESCR = "WebsitePanel Snapshot";
        private const string PROXMOX_REALM = "pam";
        private ApiClient client;
        private IRestResponse<ApiTicket> response;
        //private string upid;

        //private string node;
        private User user;
        private ProxmoxServer server;

        #endregion

        #region Constructors
        public Proxmoxvps()
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
                ApiClientSetup();
                var result = client.Status(vmId);
                var vmconfig = client.VMConfig(vmId);
                if (result != null)
                {
                    vm.Name = result.Data.name;
                    vm.Uptime = Convert.ToInt64(result.Data.uptime);
                    vm.State = getvmstate(result.Data.qmpstatus);
                    vm.CpuUsage = ConvertNullableToInt32(result.Data.cpu * 100);
                    vm.ProcessorCount = result.Data.cpus;
                    vm.CreatedDate = DateTime.Now;
                    vm.RamUsage = Convert.ToInt32(ConvertNullableToInt64(result.Data.mem / Constants.Size1M));
                    vm.RamSize = Convert.ToInt32(ConvertNullableToInt64(result.Data.maxmem / Constants.Size1M));

                    // Proxmox Generation = 0
                    vm.Generation = 0;

                    /*
                    //vm.Generation = result[0].GetInt("Generation");
                    vm.ReplicationState = result[0].GetEnum<ReplicationState>("ReplicationState");
                    */

                    if (extendedInfo)
                    {
                        vm.CpuCores = result.Data.cpus;
                        vm.HddSize = Convert.ToInt32(result.Data.maxdisk / Constants.Size1G);

                        // Hard Disk Bootdisk
                        var harddisks = ProxmoxVHDHelper.Get(vmconfig.Content);
                        foreach (var disk in harddisks)
                        {
                            if (disk.Name  == vmconfig.Data.bootdisk)
                                vm.VirtualHardDrivePath = disk.Path;
                        }
                        

                        // network adapters
                        vm.Adapters = ProxmoxNetworkHelper.Get(vmconfig.Content);

                        // DVD Drive
                        var dvdInfo = ProxmoxDvdDriveHelper.Get(client, vmId);
                        vm.DvdDriveInstalled = (dvdInfo != null);


                        /*
                        // BIOS 
                        BiosInfo biosInfo = BiosHelper.Get(PowerShell, vm.Name, vm.Generation);
                        vm.NumLockEnabled = biosInfo.NumLockEnabled;
                        */
                    }

                    //      vm.DynamicMemory = MemoryHelper.GetDynamicMemory(PowerShell, vm.Name);

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
                HostedSolutionLog.LogInfo("Proxmox Cluster Status");
                ApiClientSetup();
                var RestResponse = client.ClusterVMList();
                JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(RestResponse.Content);
                JsonArray jsonResponsearray = (JsonArray)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());

                foreach (Object vmObject in jsonResponsearray)
                {
                    JsonObject resources = (JsonObject)SimpleJson.DeserializeObject(vmObject.ToString());
                    try
                    {
                        if (resources["type"].ToString().Equals("qemu"))
                        {
                            string vmid = String.Format("{0}:{1}", resources["node"].ToString(), resources["vmid"].ToString());

                            var vm = GetVirtualMachineInternal(vmid, true);
                            vmachines.Add(vm);
                        }

                    }
                    catch (Exception ex)
                    {
                        HostedSolutionLog.LogError("GetVirtualMachines VMList", ex);
                    }

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

            // create new bitmap
            Bitmap bmp = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(bmp);

            var vm = GetVirtualMachineInternal(vmId, false);
            if (vm.State == VirtualMachineState.Running)
            {
                SolidBrush brush = new SolidBrush(Color.Black);
                g.FillRectangle(brush, 0, 0, width, height);
                RectangleF rectf = new RectangleF(0, (height / 3), width, (height / 2));

                // Set format of string.
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                // Draw string to screen.
                g.DrawString(THUMB_PRINT_TEXT, new Font("Verdana", 10), Brushes.WhiteSmoke, rectf, drawFormat);
            }
            else
            {
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
            string sshcmd = String.Format("{0} {1} {2}", DeploySSHScriptSettings, DeploySSHScriptParamsSettings, vm.OperatingSystemTemplateDeployParams);

            sshcmd = sshcmd.Replace("[FQDN]", vm.Name);
            sshcmd = sshcmd.Replace("[CPUCORES]", vm.CpuCores.ToString());
            sshcmd = sshcmd.Replace("[RAMSIZE]", vm.RamSize.ToString());
            sshcmd = sshcmd.Replace("[HDDSIZE]", vm.HddSize.ToString());
            sshcmd = sshcmd.Replace("[OSTEMPLATENAME]", vm.OperatingSystemTemplate);
            sshcmd = sshcmd.Replace("[OSTEMPLATEFILE]", vm.OperatingSystemTemplatePath);
            sshcmd = sshcmd.Replace("[ADMINPASS]", vm.AdministratorPassword);
            sshcmd = sshcmd.Replace("[VLAN]", vm.defaultaccessvlan.ToString());
            sshcmd = sshcmd.Replace("[MAC]", vm.ExternalNicMacAddress);
            if (vm.ExternalNetworkEnabled)
            {
                sshcmd = sshcmd.Replace("[IP]", vm.PrimaryIP.IPAddress);
                sshcmd = sshcmd.Replace("[NETMASK]", vm.PrimaryIP.SubnetMask);
                sshcmd = sshcmd.Replace("[GATEWAY]", vm.PrimaryIP.DefaultGateway);
            }

            // Setup Credentials and Server Information
            ConnectionInfo Conninfo = new ConnectionInfo(DeploySSHServerHostSettings, Convert.ToInt32(DeploySSHServerPortSettings), DeploySSHUserSettings,
                new AuthenticationMethod[]{

                    // Pasword based Authentication
                    new PasswordAuthenticationMethod(DeploySSHUserSettings, DeploySSHPassSettings),

                    // // Key Based Authentication (using keys in OpenSSH Format) GenerateStreamFromString
                    //new PrivateKeyAuthenticationMethod(DeploySSHUserSettings, new PrivateKeyFile(GenerateStreamFromString(DeploySSHKeySettings), DeploySSHKeyPassSettings))
                }
            );
            if (DeploySSHKeySettings != "")
            {
                Conninfo = new ConnectionInfo(DeploySSHServerHostSettings, Convert.ToInt32(DeploySSHServerPortSettings), DeploySSHUserSettings,
                    new AuthenticationMethod[]{
                        // Key Based Authentication (using keys in OpenSSH Format) GenerateStreamFromString
                        new PrivateKeyAuthenticationMethod(DeploySSHUserSettings, new PrivateKeyFile(GenerateStreamFromString(DeploySSHKeySettings), DeploySSHKeyPassSettings))
                    }
                );
            }


            try
            {
                SshClient ssh = new SshClient(Conninfo);
                try
                {
                    ssh.Connect();
                }
                catch (Exception ex)
                {
                    HostedSolutionLog.LogError("CreateVirtualMachine SSH Connection Error", ex);
                    throw;
                }
                SshCommand term = ssh.RunCommand(sshcmd);
                string output = term.Result;
                ssh.Disconnect();
                ssh.Dispose();


                // Get created machine Id
                vm.Name = vm.Name.Split('.')[0];
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

            
            UpdateConfiguration configuration = new UpdateConfiguration { };
            configuration.cores = vm.CpuCores;
            configuration.memory = vm.RamSize;

            try
            {
                var realVm = GetVirtualMachineEx(vm.VirtualMachineId);

                ApiClientSetup();
                var vmconfig = client.VMConfig(vm.VirtualMachineId);
                if (vm.HddSize != realVm.HddSize)
                     client.ResizeDisk(vm.VirtualMachineId, vmconfig.Data.bootdisk, String.Format("{0}G", vm.HddSize));
                client.UpdateConfig(vm.VirtualMachineId, configuration);
                ProxmoxDvdDriveHelper.Update(client, realVm, vm.DvdDriveInstalled);

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
            JobResult jobResult = ProxmoxJobHelper.CreateResult(ConcreteJobState.Running, ReturnCode.JobStarted);
            RestSharp.IRestResponse<Proxmox.Upid> resultclient = null;

            try
            {
                ApiClientSetup();
                switch (newState)
                {
                    case VirtualMachineRequestedState.Start:
                        resultclient = client.Start(vmId);
                        break;
                    case VirtualMachineRequestedState.Pause:
                        resultclient = client.Suspend(vmId);
                        break;
                    case VirtualMachineRequestedState.Reset:
                        resultclient = client.Reset(vmId);
                        break;
                    case VirtualMachineRequestedState.Resume:
                        resultclient = client.Resume(vmId);
                        break;
                    case VirtualMachineRequestedState.ShutDown:
                        resultclient = client.Shutdown(vmId);
                        break;
                    case VirtualMachineRequestedState.TurnOff:
                        resultclient = client.Stop(vmId);
                        break;
                    case VirtualMachineRequestedState.Save:
                        // TODO SAVE
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("newState");
                }

                
                if (resultclient.StatusCode.Equals(HttpStatusCode.OK))
                {
                    jobResult.ReturnValue = ReturnCode.JobStarted;
                }
                else
                {
                    jobResult.ReturnValue = ReturnCode.Failed;
                }

            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("ChangeVirtualMachineState", ex);
                throw;
            }
            jobResult.Job.Id = resultclient.Data.data;
            HostedSolutionLog.LogEnd("ChangeVirtualMachineState");

            return jobResult;
        }

        public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            ChangeVirtualMachineState(vmId, VirtualMachineRequestedState.TurnOff);
            return ReturnCode.OK;
        }

        public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            throw new NotImplementedException();
            /*
            //TODO104 Tasklist und nach vmId filtern
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
            */
        }

        public JobResult RenameVirtualMachine(string vmId, string name)
        {
            ApiClientSetup();
            var changedname = client.ChangeName(vmId, name);
            if (changedname.StatusCode.Equals(HttpStatusCode.OK))
            {
                return ProxmoxJobHelper.CreateSuccessResult(ReturnCode.OK);
            }
            else
            {
                return ProxmoxJobHelper.CreateSuccessResult(ReturnCode.Failed);
            }
        }

        public JobResult DeleteVirtualMachine(string vmId)
        {
            var vm = GetVirtualMachineEx(vmId);
            // The virtual computer system must be in the powered off or saved state prior to calling this method.
            if (vm.State != VirtualMachineState.Saved && vm.State != VirtualMachineState.Off)
                throw new Exception("The virtual computer system must be in the powered off or saved state prior to calling Destroy method.");
           
            ApiClientSetup();
            var deleted = client.Delete(vmId);
            if (deleted.StatusCode.Equals(HttpStatusCode.OK))
            {
                return ProxmoxJobHelper.CreateSuccessResult(ReturnCode.JobStarted);
            }
            else
            {
                return ProxmoxJobHelper.CreateSuccessResult(ReturnCode.Failed);
            }
        }

        public JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            throw new NotImplementedException();

            /*
            var vm = GetVirtualMachine(vmId);

            // The virtual computer system must be in the powered off or saved state prior to calling this method.
            if (vm.State != VirtualMachineState.Off)
                throw new Exception("The virtual computer system must be in the powered off or saved state prior to calling Export method.");

            Command cmdSet = new Command("Export-VM");
            cmdSet.Parameters.Add("Name", vm.Name);
            cmdSet.Parameters.Add("Path", FileUtils.EvaluateSystemVariables(exportPath));
            //PowerShell.Execute(cmdSet, true);
            return ProxmoxJobHelper.CreateSuccessResult(ReturnCode.JobStarted);
            */
        }

        public String GetVirtualMachineVNC(string vmId)
        {


            string result = null;
            return result;

            throw new NotImplementedException();
            /*
            // DEBUG Test von VNC!!!
            string authcookie = "NOTSETYET";
            string url = String.Format("https://{0}:{1}/?console=kvm&novnc=1&vmid={2}&vmname=wsp&node={3}|{4}", ProxmoxClusterServerHost, ProxmoxClusterServerPort, vmId, ProxmoxClusterNode, authcookie);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(url);
            string connect = Convert.ToBase64String(bytes);
            string vmvncurl = String.Format("http://{0}/vnc/vnc.php?connect={1}&resolution=", ProxmoxClusterServerHost, connect);
            return vmvncurl;
            */
        }

        #endregion

        #region Snapshots

        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            List<VirtualMachineSnapshot> snapshots = new List<VirtualMachineSnapshot>();
            try
            {
                String current_snapshot = "none";
                var vm = GetVirtualMachine(vmId);
                ApiClientSetup();
                var RestResponse = client.ListSnapshots(vmId);
                JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(RestResponse.Content);
                JsonArray jsonResponsearray = (JsonArray)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());

                foreach (Object snapshotObject in jsonResponsearray)
                {
                    JsonObject snapshot = (JsonObject)SimpleJson.DeserializeObject(snapshotObject.ToString());
                    try
                    {
                        var proxmoxsnapshot = new VirtualMachineSnapshot
                        {
                            Id = snapshot["name"].ToString(),
                            Name = null,
                            VMName = vm.Name,
                            ParentId = null,
                            Created = DateTime.Now
                        };

                        if (snapshot.ContainsKey("parent"))
                            proxmoxsnapshot.ParentId = snapshot["parent"].ToString();
                        if (snapshot.ContainsKey("description"))
                            proxmoxsnapshot.Name = snapshot["description"].ToString();

                        if (snapshot.ContainsKey("snaptime"))
                            proxmoxsnapshot.Created = ConvertFromUnixTimestamp(snapshot["snaptime"].ToString());

                        if (proxmoxsnapshot.Id.Equals("current"))
                        {
                            if (proxmoxsnapshot.ParentId != null)
                                current_snapshot = proxmoxsnapshot.ParentId.ToString();
                        }
                        else
                        {
                            snapshots.Add(proxmoxsnapshot);
                        }

                    }
                    catch (Exception ex)
                    {
                        HostedSolutionLog.LogError("GetVirtualMachineSnapshots", ex);
                    }
                    
                }
                try
                {
                    snapshots.Find(x => x.Id.Equals(current_snapshot)).IsCurrent = true;
                }
                catch (Exception ex)
                {
                    //no current snapshot found - no snapshots
                    HostedSolutionLog.LogInfo("GetVirtualMachineSnapshots no Snapshots found", ex.Message);
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetVirtualMachineSnapshots", ex);
                throw;
            }

            return snapshots;
        }

        public VirtualMachineSnapshot GetSnapshot(string vmId, string snapshotId)
        {
            try
            {
				var vm = GetVirtualMachine(vmId);
                ApiClientSetup();
                var snapshot = client.GetSnapshot(vmId, snapshotId);
                var proxmoxsnapshot = new VirtualMachineSnapshot
                {
                    Id = snapshotId,
                    Name = null,
                    VMName = vm.Name,
                    ParentId = null,
                    Created = DateTime.Now
                };
				try
				{
                    if (snapshot.Data.name != null)
                        proxmoxsnapshot.Id = snapshot.Data.name;
                    if (snapshot.Data.parent != null)
						proxmoxsnapshot.ParentId = snapshot.Data.parent;
					if (snapshot.Data.description != null)
						proxmoxsnapshot.Name = snapshot.Data.description;
					if (snapshot.Data.snaptime > 0)
						proxmoxsnapshot.Created = ConvertFromUnixTimestamp(snapshot.Data.snaptime.ToString());
				}
				catch (Exception ex)
				{
					HostedSolutionLog.LogWarning("GetSnapshot unable to set snapshot values", ex);
				}
				return proxmoxsnapshot;
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetSnapshot Error", ex);
                throw;
            }
        }

        public JobResult CreateSnapshot(string vmId)
        {
            try
            {
                var vm = GetVirtualMachine(vmId);
                string snapname = String.Format("wsp{0}uid{1}", vmId.Split(':')[1], Guid.NewGuid().ToString().GetHashCode().ToString("x"));
                string snapdescr = String.Format("{0} - {1} ({2})", vm.Name, SNAPSHOT_DESCR, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                ApiClientSetup();
                var snapshot = client.CreateSnapshot(vmId, snapname, snapdescr);
                if (snapshot.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return ProxmoxJobHelper.CreateResultUpid(snapshot.Data, ConcreteJobState.Running, ReturnCode.JobStarted);

                }
                else
                {
                    return ProxmoxJobHelper.CreateResultUpid(snapshot.Data, ConcreteJobState.Failed, ReturnCode.Failed);
                }
                
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
                ApiClientSetup();
                var renameresult = client.RenameSnapshot(vmId, snapshotId, name);
                if (renameresult.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return ProxmoxJobHelper.CreateResult(ConcreteJobState.Completed, ReturnCode.OK);

                }
                else
                {
                    return ProxmoxJobHelper.CreateResult(ConcreteJobState.Failed, ReturnCode.Failed);
                }
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
                ApiClientSetup();
                var applyresult = client.rollback(vmId, snapshotId);
                if (applyresult.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return ProxmoxJobHelper.CreateResultUpid(applyresult.Data, ConcreteJobState.Running, ReturnCode.JobStarted);

                }
                else
                {
                    return ProxmoxJobHelper.CreateResultUpid(applyresult.Data, ConcreteJobState.Failed, ReturnCode.Failed);
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("ApplySnapshot", ex);
                throw;
            }
        }

        public JobResult DeleteSnapshot(string vmId, string snapshotId)
        {
            JobResult jobResult = ProxmoxJobHelper.CreateResult(ConcreteJobState.Running, ReturnCode.JobStarted);
            try
            {
                ApiClientSetup();
                var deleteresult = client.DeleteSnapshot(vmId, snapshotId);
                JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(deleteresult.Content);
                String jobid = jsonResponse["data"].ToString();

                if (deleteresult.StatusCode.Equals(HttpStatusCode.OK))
                {
                    jobResult = ProxmoxJobHelper.CreateResult(ConcreteJobState.Running, ReturnCode.JobStarted);

                }
                else
                {
                    jobResult = ProxmoxJobHelper.CreateResult(ConcreteJobState.Failed, ReturnCode.Failed);
                }
                jobResult.Job.Id = jobid;
                
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("DeleteSnapshot", ex);
                throw;
            }
            return jobResult;
        }

        public JobResult DeleteSnapshotSubtree(string vmId, string snapshotId)
        {
            try
            {
                return DeleteSnapshot(vmId, snapshotId);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("DeleteSnapshot", ex);
                throw;
            }
        }

        public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            return null;
        }

        #endregion

        #region DVD operations
        public LibraryItem[] GetDVDISOs(string vmId)
        {
            List<LibraryItem> imagelist = new List<LibraryItem>();
            try
            {
                ApiClientSetup();
                var RestResponse = client.ListISOs(vmId, ProxmoxIsosonStorage);
                JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(RestResponse.Content);
                JsonArray jsonResponsearray = (JsonArray)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());
                foreach (Object dvdobj in jsonResponsearray)
                {
                    JsonObject dvd = (JsonObject)SimpleJson.DeserializeObject(dvdobj.ToString());
                    try
                    {
                        if (dvd["content"].ToString() == "iso" && dvd["format"].ToString() == "iso")
                        {
                            LibraryItem item = new LibraryItem();
                            item.Path = dvd["volid"].ToString();
                            string isoname = dvd["volid"].ToString().Split('/')[1];
                            if (isoname == "")
                                isoname = dvd["volid"].ToString().Split('/')[0];
                            item.Name = isoname.Replace(".iso", "");
                            item.DiskSize = Convert.ToInt32(dvd["size"].ToString());
                            imagelist.Add(item);
                        }

                    }
                    catch (Exception ex)
                    {
                        HostedSolutionLog.LogWarning("GetDVDISOs - unable to add DVD to List", ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogWarning("GetDVDISOs - unable to Get DVD ISO List", ex.Message);
            }
            return imagelist.ToArray();
        }

        public string GetInsertedDVD(string vmId)
        {
            HostedSolutionLog.LogStart("GetInsertedDVD");
            HostedSolutionLog.DebugInfo("Virtual Machine: {0}", vmId);

            ApiClientSetup();
            DvdDriveInfo dvdInfo;
            try
            {
                dvdInfo = ProxmoxDvdDriveHelper.Get(client, vmId);
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

            int disksize = 0;
            
            // load library items
            LibraryItem[] disks = GetDVDISOs(vmId);

            // find required disk
            foreach (LibraryItem disk in disks)
            {
                if (String.Compare(isoPath, disk.Path, true) == 0)
                    disksize = disk.DiskSize;
            }

            ApiClientSetup();
            try
            {
                ProxmoxDvdDriveHelper.Set(client, vmId, isoPath, disksize);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("InsertDVD", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("InsertDVD");
            return ProxmoxJobHelper.CreateSuccessResult();
        }

        public JobResult EjectDVD(string vmId)
        {
          
            HostedSolutionLog.LogStart("InsertDVD");
            HostedSolutionLog.DebugInfo("Virtual Machine: {0}", vmId);

            ApiClientSetup();
            try
            {
                ProxmoxDvdDriveHelper.Set(client, vmId, null, 0);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("InsertDVD", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("InsertDVD");
            return ProxmoxJobHelper.CreateSuccessResult();
        }
        #endregion

        #region Virtual Switches
        public List<VirtualSwitch> GetSwitches()
        {
            return GetSwitches(null, null);
        }

        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            return GetSwitches(computerName, "External");
        }

        private List<VirtualSwitch> GetSwitches(string computerName, string type)
        {
            throw new NotImplementedException();
            /*
            HostedSolutionLog.LogStart("GetSwitches");
            HostedSolutionLog.DebugInfo("ComputerName: {0}", computerName);

            List<VirtualSwitch> switches = new List<VirtualSwitch>();

            try
            {
                
                Command cmd = new Command("Get-VMSwitch");

                // Not needed as the PowerShellManager adds the computer name
                if (!string.IsNullOrEmpty(computerName)) cmd.Parameters.Add("ComputerName", computerName);
                if (!string.IsNullOrEmpty(type)) cmd.Parameters.Add("SwitchType", type);

                Collection<PSObject> result = PowerShell.Execute(cmd, false, true);


                foreach (PSObject current in result)
                {
                    VirtualSwitch sw = new VirtualSwitch();
                    sw.SwitchId = current.GetProperty("Name").ToString();
                    sw.Name = current.GetProperty("Name").ToString();
                    sw.SwitchType = current.GetProperty("SwitchType").ToString();
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
            */
        }

        public bool SwitchExists(string switchId)
        {
            return GetSwitches().Any(s => s.Name == switchId);
        }

        public VirtualSwitch CreateSwitch(string name)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

        public ReturnCode DeleteSwitch(string switchId) // switchId is SwitchName
        {
            return ReturnCode.OK;
            /*
            throw new NotImplementedException();
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
            */
        }
        #endregion

        #region KVP - NotImplemented
        public List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            throw new NotImplementedException();
            //return GetKVPItems(vmId, "GuestExchangeItems");
        }

        public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            throw new NotImplementedException();
            //return GetKVPItems(vmId, "GuestIntrinsicExchangeItems");
        }

        private List<KvpExchangeDataItem> GetKVPItems(string vmId, string exchangeItemsName)
        {
            throw new NotImplementedException();
            /*
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
            */

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
            throw new NotImplementedException();
            /*
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
            */
        }

        public JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

        public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            throw new NotImplementedException();
            /*
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
            */
        }
        #endregion

        #region Storage - NotImplemented
        public VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            throw new NotImplementedException();
            /*
            try
            {
                VirtualHardDiskInfo hardDiskInfo = new VirtualHardDiskInfo();
                HardDriveHelper.GetVirtualHardDiskDetail(PowerShell, vhdPath, ref hardDiskInfo);
                return hardDiskInfo;
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetVirtualHardDiskInfo", ex);
                throw;
            }
            */
        }

        public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

        public ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            throw new NotImplementedException();
            /*
            Command cmd = new Command("Dismount-VHD");

            cmd.Parameters.Add("Path", FileUtils.EvaluateSystemVariables(vhdPath));

            PowerShell.Execute(cmd, true);
            return ReturnCode.OK;
            */
        }

        public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            throw new NotImplementedException();
            /*
            Command cmd = new Command("Resize-VHD");

            cmd.Parameters.Add("Path", FileUtils.EvaluateSystemVariables(vhdPath));
            cmd.Parameters.Add("SizeBytes", sizeGB * Constants.Size1G);

            PowerShell.Execute(cmd, true, true);
            return ProxmoxJobHelper.CreateSuccessResult(ReturnCode.JobStarted);
            */
        }

        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            throw new NotImplementedException();
            /*
            // check source file
            if (!FileExists(sourcePath))
                throw new Exception("Source VHD cannot be found: " + sourcePath);

            // check destination folder
            string destFolder = Path.GetDirectoryName(destinationPath);
            if (!DirectoryExists(destFolder))
                CreateFolder(destFolder);
            
            sourcePath = FileUtils.EvaluateSystemVariables(sourcePath);
            destinationPath = FileUtils.EvaluateSystemVariables(destinationPath); 
            
            try
            {
                Command cmd = new Command("Convert-VHD");

                cmd.Parameters.Add("Path", sourcePath);
                cmd.Parameters.Add("DestinationPath", destinationPath);
                cmd.Parameters.Add("VHDType", diskType.ToString());

                PowerShell.Execute(cmd, true, true);
                return ProxmoxJobHelper.CreateSuccessResult(ReturnCode.JobStarted);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("ConvertVirtualHardDisk", ex);
                throw;
            }
            */
        }

        public void DeleteRemoteFile(string path)
        {
            throw new NotImplementedException();
            /*
            if (DirectoryExists(path))
                DeleteFolder(path); // WMI way
            else if (FileExists(path))
                DeleteFile(path); // WMI way
            */
        }

        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

       
        public string ReadRemoteFile(string path)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

        public void WriteRemoteFile(string path, string content)
        {
            throw new NotImplementedException();
            /*
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
            */
        }
        #endregion - NotImplemented

        #region Jobs
        public ConcreteJob GetJob(string jobId)
        {
            ConcreteJob Job = new ConcreteJob();
            ApiClientSetup();
            var taskstatus = client.TaskStatus(jobId);
            Job.Id = taskstatus.Data.upid;
            Job.StartTime = ConvertFromUnixTimestamp(taskstatus.Data.starttime.ToString());
            Job.JobState = getjobstate(taskstatus.Data.status);
            Job.PercentComplete = 0;
            Job.ErrorDescription = taskstatus.Data.exitstatus;
            if (taskstatus.Data.exitstatus != null && taskstatus.Data.exitstatus != "" && taskstatus.Data.exitstatus != "OK")
                Job.JobState = ConcreteJobState.Failed;
            return Job;
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
            /*
            ApiClientSetup();
            var nodestatus = client.NodeStatus();
            return 32;
            return Convert.ToInt32(nodestatus.Data.cpuinfo.cpus);
            */

            HostedSolutionLog.LogStart("GetProcessorCoresNumber");
            int cores = 0;
            int nodemaxcpu;

            List<VirtualMachine> vmachines = new List<VirtualMachine>();
            try
            {
                HostedSolutionLog.LogInfo("Proxmox Cluster Status");

                ApiClientSetup();
                var RestResponse = client.ClusterResources();
                JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(RestResponse.Content);
                JsonArray jsonResponsearray = (JsonArray)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());

                foreach (Object vmObject in jsonResponsearray)
                {
                    JsonObject resources = (JsonObject)SimpleJson.DeserializeObject(vmObject.ToString());
                    try
                    {
                        if (resources["type"].ToString().Equals("node"))
                        {
                            nodemaxcpu = Convert.ToInt32(resources["maxcpu"].ToString());
                            if (nodemaxcpu > cores)
                                cores = nodemaxcpu;
                        }

                    }
                    catch (Exception ex)
                    {
                        HostedSolutionLog.LogError("GetProcessorCoresNumber", ex);
                    }

                }

                HostedSolutionLog.LogInfo("Finish");


            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("GetProcessorCoresNumber", ex);
                throw;
            }

            HostedSolutionLog.LogEnd("GetProcessorCoresNumber");
            return cores;

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
            // no installation required
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
                        ReturnCode code = ShutDownVirtualMachine(vm.VirtualMachineId, true, "Virtual Machine has been suspended from WebsitePanel");
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
            throw new NotImplementedException();
        }


        private bool JobCompleted(ConcreteJob job)
        {
            //OK
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

        #endregion

        #region Remote File Methods - NotImplemented
        public bool FileExists(string path)
        {
            throw new NotImplementedException();
            /*
            HostedSolutionLog.LogInfo("Check remote file exists: " + path);

            if (path.StartsWith(@"\\")) // network share
                return File.Exists(path);
            else
            {
                Wmi cimv2 = new Wmi(ServerNameSettings, Constants.WMI_CIMV2_NAMESPACE);
                ManagementObject objFile = cimv2.GetWmiObject("CIM_Datafile", "Name='{0}'", path.Replace("\\", "\\\\"));
                return (objFile != null);
            }
            */
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
            /*
            if (path.StartsWith(@"\\")) // network share
                return Directory.Exists(path);
            else
            {
                Wmi cimv2 = new Wmi(ServerNameSettings, Constants.WMI_CIMV2_NAMESPACE);
                ManagementObject objDir = cimv2.GetWmiObject("Win32_Directory", "Name='{0}'", path.Replace("\\", "\\\\"));
                return (objDir != null);
            }
            */
        }

        public bool CopyFile(string sourceFileName, string destinationFileName)
        {
            throw new NotImplementedException();
            /*
            HostedSolutionLog.LogInfo("Copy file - source: " + sourceFileName);
            HostedSolutionLog.LogInfo("Copy file - destination: " + destinationFileName);

            if (sourceFileName.StartsWith(@"\\")) // network share
            {
                if (!File.Exists(sourceFileName))
                    return false;

                //File.Copy(sourceFileName, destinationFileName);
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
            */
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
            /*
            if (path.StartsWith(@"\\"))
            {
                // network share
                //File.Delete(path);
            }
            else
            {
                // delete file using WMI
                //Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");
                //ManagementObject objFile = cimv2.GetWmiObject("CIM_Datafile", "Name='{0}'", path.Replace("\\", "\\\\"));
                //objFile.InvokeMethod("Delete", null);
            }
            */
        }

        public void DeleteFolder(string path)
        {
            throw new NotImplementedException();
        }

        private ManagementObjectCollection GetSubFolders(string path)
        {
            throw new NotImplementedException();
            /*
            if (path.EndsWith("\\"))
                path = path.Substring(0, path.Length - 1);

            Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");

            return cimv2.ExecuteWmiQuery("Associators of {Win32_Directory.Name='"
                + path + "'} "
                + "Where AssocClass = Win32_Subdirectory "
                + "ResultRole = PartComponent");
            */
            //return null;
        }

        public void CreateFolder(string path)
        {
            throw new NotImplementedException();
            //VdsHelper.ExecuteRemoteProcess(ServerNameSettings, String.Format("cmd.exe /c md \"{0}\"", path));
        }


        #endregion

        #region Hyper-V Cloud
        public bool CheckServerState(string connString)
        {
            return !String.IsNullOrEmpty(connString);
        }
        #endregion Hyper-V Cloud

        #region Replication - NotImplemented
        public List<CertificateInfo> GetCertificates(string remoteServer)
        {
            return null;
        }

        public void SetReplicaServer(string remoteServer, string thumbprint, string storagePath)
        {
            throw new NotImplementedException();
        }

        public void UnsetReplicaServer(string remoteServer)
        {
            throw new NotImplementedException();
        }

        public ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            throw new NotImplementedException();
        }

        public void EnableVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            throw new NotImplementedException();
        }

        public void SetVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            throw new NotImplementedException();
        }

        public void TestReplicationServer(string vmId, string replicaServer, string localThumbprint)
        {
            throw new NotImplementedException();
        }

        public void StartInitialReplication(string vmId)
        {
            throw new NotImplementedException();
        }

        public VmReplication GetReplication(string vmId)
        {
            throw new NotImplementedException();
        }

        public void DisableVmReplication(string vmId)
        {
            throw new NotImplementedException();
        }


        public ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            throw new NotImplementedException();
        }

        public void PauseReplication(string vmId)
        {
            throw new NotImplementedException();
        }

        public void ResumeReplication(string vmId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Proxmox Apiclient Setup
        public void ApiClientSetup()
        {
            
            string clusterrealm = PROXMOX_REALM;
            if (ProxmoxClusterRealm != null && ProxmoxClusterRealm != "")
               clusterrealm = ProxmoxClusterRealm;

            user = new User { Username = ProxmoxClusterAdminUser, Password = ProxmoxClusterAdminPass, Realm = clusterrealm };
            server = new ProxmoxServer { Ip = ProxmoxClusterServerHost, Port = ProxmoxClusterServerPort };

            //client = new ApiClient(server, ProxmoxClusterNode);
            client = new ApiClient(server);


            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

            response = client.Login(user);

        }

        #endregion


        #region Linux to Windows States & Formats
        private VirtualMachineState getvmstate(string state)
        {
            VirtualMachineState vmstate;
            switch (state)
            {
                case "running":
                    vmstate = VirtualMachineState.Running;
                    break;
                case "stopped":
                    vmstate = VirtualMachineState.Off;
                    break;
                case "paused":
                    vmstate = VirtualMachineState.Paused;
                    break;
                case "suspended":
                    vmstate = VirtualMachineState.Paused;
                    break;
                default:
                    vmstate = VirtualMachineState.Other;
                    break;
            }
            return vmstate;
        }

        private ConcreteJobState getjobstate(string state)
        {
            ConcreteJobState jobstate;
            switch (state)
            {
                case "running":
                    jobstate = ConcreteJobState.Running;
                    break;
                case "starting":
                    jobstate = ConcreteJobState.Starting;
                    break;
                case "stopped":
                    jobstate = ConcreteJobState.Completed;
                    break;
                default:
                    jobstate = ConcreteJobState.Completed;
                    break;
            }
            return jobstate;
        }

        static DateTime ConvertFromUnixTimestamp(String timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            Double timestampdouble = Convert.ToDouble(timestamp);
            return origin.AddSeconds(timestampdouble);
        }


        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        #endregion

    }
}