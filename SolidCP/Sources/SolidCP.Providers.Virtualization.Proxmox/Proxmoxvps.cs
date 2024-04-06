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
using SkiaSharp;

using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

using System.Reflection;
using System.Globalization;
using System.Runtime.InteropServices;

using RestSharp;
using System.Net;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Threading;

using System.Xml;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolidCP.Providers.Virtualization.Extensions;
using SolidCP.Providers.Virtualization.Proxmox;

using Renci.SshNet;
using Renci.SshNet.Common;
using Corsinvest.ProxmoxVE.Api;
using Corsinvest.ProxmoxVE.Api.Shared;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace SolidCP.Providers.Virtualization
{
    public class Proxmoxvps : HostingServiceProviderBase, IVirtualizationServerProxmox, IDisposable
    {

        #region Provider Settings
        public virtual string ProxmoxClusterServerHost
        {
            get { return ProviderSettings["ProxmoxClusterServerHost"]; }
        }

        public virtual string ProxmoxClusterServerApiHost => ProxmoxClusterServerHost;

        public string ProxmoxClusterServerPort
        {
            get { return ProviderSettings["ProxmoxClusterServerPort"]; }
        }

        public string ProxmoxClusterAdminUser
        {
            get { return ProviderSettings["ProxmoxClusterAdminUser"]; }
        }

        public string ProxmoxClusterRealm
        {
            get { return ProviderSettings["ProxmoxClusterRealm"]; }
        }

        public string ProxmoxClusterAdminPass
        {
            get { return ProviderSettings["ProxmoxClusterAdminPass"]; }
        }
        public bool? ProxmoxTrustClusterServerCertificate
        {
            get
            {
                var setting = ProviderSettings["ProxmoxTrustClusterServerCertificate"];
                if (string.IsNullOrEmpty(setting)) return null;
                bool trustCert = false;
                bool.TryParse(setting, out trustCert);
                return trustCert;
            }
        }

        static string node = null;
        public virtual string ProxmoxClusterNode
        {
            get
            {
                if (node == null)
                {
                    node = "";
                    try
                    {
                        var ssh = SshClient();
                        ssh.Connect();
                        var term = ssh.RunCommand("hostname");
                        node = term.Result;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return node;
            }
        }
        /*
		protected string ProxmoxClusterNode
		{
			 get { return ProviderSettings["ProxmoxClusterNode"]; }
		}
		*/

        public string ProxmoxIsosonStorage
        {
            get { return ProviderSettings["ProxmoxIsosonStorage"]; }
        }

        public string ServerNameSettings
        {
            get { return ProviderSettings["ServerName"]; }
        }

        public string DeploySSHServerHostSettings
        {
            get { return ProviderSettings["DeploySSHServerHost"]; }
        }

        public string DeploySSHServerPortSettings
        {
            get { return ProviderSettings["DeploySSHServerPort"]; }
        }

        public string DeploySSHUserSettings
        {
            get { return ProviderSettings["DeploySSHUser"]; }
        }

        public string DeploySSHPassSettings
        {
            get { return ProviderSettings["DeploySSHPass"]; }
        }

        public string DeploySSHKeySettings
        {
            get { return ProviderSettings["DeploySSHKey"]; }
        }

        public string DeploySSHKeyPassSettings
        {
            get { return ProviderSettings["DeploySSHKeyPass"]; }
        }

        public string DeploySSHScriptSettings
        {
            get { return ProviderSettings["DeploySSHScript"]; }
        }

        public string DeploySSHScriptParamsSettings
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
        private const string SNAPSHOT_DESCR = "SolidCP Snapshot";
        private const string PROXMOX_REALM = "pam";
        public RestResponse Response;
        //private string upid;


        public virtual bool IsLocalServer => IsHostLocal(ProxmoxClusterServerApiHost);
        public virtual bool ValidateServerCertificate => !IsLocalServer ||
            !ProxmoxTrustClusterServerCertificate.HasValue || !ProxmoxTrustClusterServerCertificate.Value;

        ProxmoxServer server;
        public ProxmoxServer Server => server ?? (server = new ProxmoxServer { Ip = string.IsNullOrEmpty(ProxmoxClusterServerApiHost) ? "127.0.0.1" : ProxmoxClusterServerApiHost, Port = ProxmoxClusterServerPort, ValidateCertificate = ValidateServerCertificate });

        ApiClient api = null;
        public ApiClient Api
        {
            get
            {
                if (api != null) return api;
                {
                    api = new ApiClient(this);
                    // TODO support certificate validation
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    api.Login(User);
                }
                return api;
            }
        }

        #endregion

        #region Constructors
        public Proxmoxvps()
        {
            LoadSkiaNativeDlls();
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
            HostedSolutionLog.DebugInfo("GetVirtualMachine - Virtual Machine: {0}", vmId);

            VirtualMachine vm = new VirtualMachine();
            vm.VirtualMachineId = vmId;
            try
            {
                var result = Api.Status(vmId);
                HostedSolutionLog.DebugInfo("GetVirtualMachine - Virtual Machine: {0}", vmId);
                var vmconfig = Api.VMConfig(vmId);
                if (result != null)
                {
                    HostedSolutionLog.DebugInfo("GetVirtualMachineInternal - vm.Name: {0}, State: {1}, Uptime: {2}", result.data.name, result.data.qmpstatus, result.data.uptime);
                    vm.Name = result.data.name;
                    vm.Uptime = result.data.uptime;
                    string qmpstatus = result.data.qmpstatus;
                    vm.State = GetVMState(qmpstatus);
                    vm.CpuUsage = ConvertNullableToInt32(result.data.cpu * 100);
                    vm.ProcessorCount = result.data.cpus;
                    vm.CreatedDate = DateTime.Now;
                    Int64 mem = result.data.mem;
                    vm.RamUsage = Convert.ToInt32(ConvertNullableToInt64(mem / Constants.Size1M));
                    Int64 maxmem = result.data.maxmem;
                    vm.RamSize = Convert.ToInt32(ConvertNullableToInt64(maxmem / Constants.Size1M));

                    // Proxmox Generation = 0
                    vm.Generation = 0;

                    /*
					//vm.Generation = result[0].GetInt("Generation");
					vm.ReplicationState = result[0].GetEnum<ReplicationState>("ReplicationState");
					*/

                    if (extendedInfo)
                    {
                        HostedSolutionLog.DebugInfo("GetVirtualMachineInternal - extended - CpuCores, {0} maxdisk: {1}", result.data.cpus, result.data.maxdisk);
                        vm.CpuCores = result.data.cpus;
                        Int64 maxdisk = result.data.maxdisk;
                        vm.HddSize = new[] { Convert.ToInt32(maxdisk / Constants.Size1G) };

                        //vmconfig
                        //JsonObject vmconfigjsonResponse = (JsonObject)SimpleJson.DeserializeObject(vmconfig.Content);
                        //dynamic vmconfigconfigvalue = (JsonObject)SimpleJson.DeserializeObject(vmconfigjsonResponse["data"].ToString());
                        JToken vmconfigjsonResponse = JToken.Parse(vmconfig.Content);
                        JObject vmconfigconfigvalue = (JObject)vmconfigjsonResponse["data"];

                        // Checking for bootdisk as newer VMs use boot not bootorder
                        string bootdisk;
                        if (!vmconfigconfigvalue.ContainsKey("bootdisk"))
                        {
                            string boot1 = (string)vmconfigconfigvalue["boot"];
                            var bootvar = boot1.Replace("order=", "").Split(';');
                            bootdisk = bootvar[0];
                        }
                        else
                        {
                            bootdisk = (string)vmconfigconfigvalue["bootdisk"];
                        };


                        // Hard Disk Bootdisk
                        var harddisks = ProxmoxVHDHelper.Get(vmconfig.Content);
                        foreach (var disk in harddisks)
                        {
                            if (disk.Name == bootdisk)
                                vm.VirtualHardDrivePath = new[] { disk.Path };
                            HostedSolutionLog.DebugInfo("GetVirtualMachineInternal - VirtualHardDrivePath {0}, Name: {1}, bootdisk {2}", disk.Path, disk.Name, bootdisk);
                        }


                        // network adapters
                        vm.Adapters = ProxmoxNetworkHelper.Get(vmconfig.Content);

                        // DVD Drive
                        var dvdInfo = ProxmoxDvdDriveHelper.Get(Api, vmId);
                        vm.DvdDriveInstalled = (dvdInfo != null);


                        /*
						// BIOS 
						BiosInfo biosInfo = BiosHelper.Get(PowerShell, vm.Name, vm.Generation);
						vm.NumLockEnabled = biosInfo.NumLockEnabled;
						*/
                    }

                    //      vm.DynamicMemory = MemoryHelper.GetDynamicMemory(PowerShell, vm.Name);

                    HostedSolutionLog.LogInfo("GetVirtualMachine - vm.name: {0}", vm.Name);

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
                //HostedSolutionLog.LogInfo("GetVirtualMachines - Proxmox Cluster Status");
                //HostedSolutionLog.LogInfo("GetVirtualMachines - APIClientSetup Ran");
                var RestResponse = Api.ClusterVMList();
                //HostedSolutionLog.LogInfo("GetVirtualMachines - ClusterVMList: {0}", RestResponse.Content.ToString());
                //JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(RestResponse.Content);
                //JsonArray jsonResponsearray = (JsonArray)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());
                JToken jsonResponse = JToken.Parse(RestResponse.Content);
                JArray jsonResponsearray = (JArray)jsonResponse["data"];


                foreach (JObject resources in jsonResponsearray)
                {
                    try
                    {
                        if (((string)resources["type"]).Equals("qemu"))
                        {
                            string vmid = String.Format("{0}:{1}", (string)resources["node"], (string)resources["vmid"]);
                            HostedSolutionLog.LogInfo("GetVirtualMachines - vmObject: {0}", vmid);

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
        public bool IsLinuxMusl
        {
            get
            {
                if (!OSInfo.IsLinux) return false;
                return OS.Shell.Default.Exec("ldd /bin/ls").OutputAndError().Result.Contains("musl");
            }
        }

        Dictionary<string, IntPtr> loadedNativeDlls = new Dictionary<string, IntPtr>();
        public IntPtr SkiaDllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName.Contains("SkiaSharp"))
            {
                lock (this)
                {
                    IntPtr dll;
                    if (loadedNativeDlls.TryGetValue(libraryName, out dll)) return dll;

                    var runtimeInformation = typeof(RuntimeInformation);
                    var runtimeIdentifier = (string?)runtimeInformation.GetProperty("RuntimeIdentifier")?.GetValue(null);
                    if (runtimeIdentifier == "linux-x64" && IsLinuxMusl) runtimeIdentifier = "linux-musl-x64";
                    runtimeIdentifier = runtimeIdentifier.Replace("linux-", "");
                    var currentDllPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                    string libraryFileName = libraryName;
                    if (!libraryFileName.EndsWith(".so")) libraryFileName += ".so";
                    if (!libraryFileName.StartsWith("lib")) libraryFileName = "lib" + libraryFileName;
                    var nativeDllPath = Path.Combine(currentDllPath, runtimeIdentifier, libraryFileName);

                    if (File.Exists(nativeDllPath))
                    {
                        // call NativeLibrary.Load via reflection, becuase it's not available in NET Standard
                        var nativeLibrary = Type.GetType("System.Runtime.InteropServices.NativeLibrary, System.Runtime.InteropServices");
                        var load = nativeLibrary.GetMethod("Load", new Type[] { typeof(string), typeof(Assembly), typeof(DllImportSearchPath?) });
                        dll = (IntPtr)load?.Invoke(null, new object[] { nativeDllPath, assembly, searchPath });
                        loadedNativeDlls.Add(libraryName, dll);

                        Console.WriteLine($"Loaded native library: {nativeDllPath}");

                        return dll;
                    }
                }
            }

            // Otherwise, fallback to default import resolver.
            return IntPtr.Zero;
        }


        static bool nativeSkiaDllLoaded = false;
        public void LoadSkiaNativeDlls()
        {
            if (nativeSkiaDllLoaded) return;
            nativeSkiaDllLoaded = true;

            if (OSInfo.IsLinux)
            {
                // call NativeLibrary.SetDllImportResolver via reflection, becuase it's not available in NET Standard
                var nativeLibrary = Type.GetType("System.Runtime.InteropServices.NativeLibrary, System.Runtime.InteropServices");
                var dllImportResolver = Type.GetType("System.Runtime.InteropServices.DllImportResolver, System.Runtime.InteropServices");

                Assembly skiaSharp = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == "SkiaSharp");
                if (skiaSharp == null)
                {
                    skiaSharp = Assembly.Load("SkiaSharp");
                }
                var setDllImportResolver = nativeLibrary.GetMethod("SetDllImportResolver", new Type[] { typeof(Assembly), dllImportResolver });
                //var importResolverMethod = this.GetType().GetMethod(nameof(SkiaDllImportResolver));

                var skiaDllImportResolver = Delegate.CreateDelegate(dllImportResolver, this, nameof(SkiaDllImportResolver));
                setDllImportResolver?.Invoke(null, new object[] { skiaSharp, skiaDllImportResolver });

                Console.WriteLine("Added SkiaSharp DllImportResolver");
            }
        }

        public ImageFile GetVirtualMachineThumbnailImageScreenshot(string vmId, int width, int height)
        {
            SKImage image = null;
            try
            {
                image = (SKImage)Api.GetScreenshot(vmId);
            }
            catch (Exception ex)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var svgName = assembly.GetManifestResourceNames()
                    .First(name => name.EndsWith("stop-sign-svgrepo-com.svg", StringComparison.OrdinalIgnoreCase));
                using (var res = assembly.GetManifestResourceStream(svgName))
                {
                    var bytes = new byte[res.Length];
                    res.Read(bytes, 0, (int)res.Length);
                    return new ImageFile()
                    {
                        MimeType = "image/svg+xml",
                        FileExtension = "svg",
                        RawData = bytes
                    };
                }
            }

            SKImageInfo thumbinfo = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Opaque);
            SKImage thumb = SKImage.Create(thumbinfo);
            image?.ScalePixels(thumb.PeekPixels(), SKFilterQuality.Medium);

            MemoryStream stream = new MemoryStream();

            using (var data = thumb.Encode(SKEncodedImageFormat.Png, 100))
            {
                data.SaveTo(stream);
            }
            stream.Flush();
            var buffer = stream.ToArray();

            var imageFile = new ImageFile()
            {
                FileExtension = "png",
                MimeType = "image/png",
                RawData = buffer
            };
            return imageFile;
        }

        public ImageFile GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
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

            return GetVirtualMachineThumbnailImageScreenshot(vmId, width, height);
        }

        public virtual SshClient SshClient()
        {
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
                return ssh;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual VirtualMachine CreateVirtualMachine(VirtualMachine vm)
        {
            string sshcmd = String.Format("\"{0}\" {1} {2}", DeploySSHScriptSettings, DeploySSHScriptParamsSettings, vm.OperatingSystemTemplateDeployParams);

            sshcmd = sshcmd.Replace("[FQDN]", vm.Name);
            sshcmd = sshcmd.Replace("[CPUCORES]", vm.CpuCores.ToString());
            sshcmd = sshcmd.Replace("[RAMSIZE]", vm.RamSize.ToString());
            sshcmd = sshcmd.Replace("[HDDSIZE]", vm.HddSize[0].ToString());
            sshcmd = sshcmd.Replace("[OSTEMPLATENAME]", $"\"{vm.OperatingSystemTemplate}\"");
            sshcmd = sshcmd.Replace("[OSTEMPLATEFILE]", $"\"{vm.OperatingSystemTemplatePath}\"");
            sshcmd = sshcmd.Replace("[ADMINPASS]", $"\"{vm.AdministratorPassword}\"");
            sshcmd = sshcmd.Replace("[VLAN]", vm.DefaultAccessVlan.ToString());
            sshcmd = sshcmd.Replace("[MAC]", vm.ExternalNicMacAddress);
            if (vm.ExternalNetworkEnabled)
            {
                sshcmd = sshcmd.Replace("[IP]", vm.PrimaryIP.IPAddress);
                sshcmd = sshcmd.Replace("[NETMASK]", vm.PrimaryIP.SubnetMask);
                sshcmd = sshcmd.Replace("[GATEWAY]", vm.PrimaryIP.DefaultGateway);
            }
            else
            {
                sshcmd = sshcmd.Replace("[IP]", "\"\"");
                sshcmd = sshcmd.Replace("[NETMASK]", "\"\"");
                sshcmd = sshcmd.Replace("[GATEWAY]", "\"\"");
            }

            string error = "Error creating wirtual machine.";
            try
            {
                SshClient ssh = SshClient();
                try
                {
                    ssh.Connect();
                }
                catch (Exception ex)
                {
                    HostedSolutionLog.LogError("Error creating virtual machine SSH connection error", ex);
                    throw;
                }
                var term = ssh.CreateCommand($"sudo -n {sshcmd}");
                term.CommandTimeout = TimeSpan.FromMinutes(120);
                term.Execute();
                string output = term.Result;
                string cmdError = term.Error;
                error = $"Error creating wirtual machine. VM deploy script output:\n{cmdError}\n{output}";
                ssh.Disconnect();
                ssh.Dispose();


                // Get created machine Id
                vm.Name = vm.Name.Split('.')[0];

                var createdMachine = GetVirtualMachines().FirstOrDefault(m => m.Name == vm.Name);
                if (createdMachine == null)
                {
                    error = $"Can't find created machine. VM deploy script output:\n{cmdError}\n{output}";
                    var ex = new Exception(error);

                    HostedSolutionLog.LogError(error, ex);
                    throw ex;
                }
                vm.VirtualMachineId = createdMachine.VirtualMachineId;

                // Update common settings
                UpdateVirtualMachine(vm);
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(error, ex);
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

                var vmconfig = Api.VMConfig(vm.VirtualMachineId);
                if (vm.HddSize[0] != realVm.HddSize[0])
                    Api.ResizeDisk(vm.VirtualMachineId, vmconfig.Data.bootdisk, $"{vm.HddSize[0]}G");
                Api.UpdateConfig(vm.VirtualMachineId, configuration);
                ProxmoxDvdDriveHelper.Update(Api, realVm, vm.DvdDriveInstalled);
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
            RestResponse<Proxmox.Upid> resultclient = null;

            try
            {
                switch (newState)
                {
                    case VirtualMachineRequestedState.Start:
                        resultclient = Api.Start(vmId);
                        break;
                    case VirtualMachineRequestedState.Pause:
                        resultclient = Api.Suspend(vmId);
                        break;
                    case VirtualMachineRequestedState.Reset:
                        resultclient = Api.Reset(vmId);
                        break;
                    case VirtualMachineRequestedState.Resume:
                        resultclient = Api.Resume(vmId);
                        break;
                    case VirtualMachineRequestedState.ShutDown:
                        resultclient = Api.Shutdown(vmId);
                        break;
                    case VirtualMachineRequestedState.TurnOff:
                        resultclient = Api.Stop(vmId);
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
            var changedname = Api.ChangeName(vmId, name);
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

            var deleted = Api.Delete(vmId);
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

        static Dictionary<string, System.Net.IPAddress[]> ResolvedHosts = new Dictionary<string, System.Net.IPAddress[]>();

        public bool IsLocalAddress(string adr)
        {
            return Regex.IsMatch(adr, @"(^127\.)|(^192\.168\.)|(^10\.)|(^172\.1[6-9]\.)|(^172\.2[0-9]\.)|(^172\.3[0-1]\.)|(^\[?::1\]?$)|(^\[?[fF][cCdD])", RegexOptions.Singleline);
        }

        public bool IsHostLocal(string host)
        {
            if (string.IsNullOrEmpty(host)) return true;

            var isHostIP = Regex.IsMatch(host, @"^[0.9]{1,3}(?:\.[0-9]{1,3}){3}$", RegexOptions.Singleline) || Regex.IsMatch(host, @"^\[?[0-9a-fA-F:]+\]?$", RegexOptions.Singleline);
            if (host == "localhost" || host == "127.0.0.1" || host == "::1" || host == "[::1]" || !isHostIP && !host.Contains('.') ||
                isHostIP && IsLocalAddress(host)) return true;

            if (!isHostIP)
            {
                IPAddress[] ips;
                lock (ResolvedHosts)
                {
                    if (!ResolvedHosts.TryGetValue(host, out ips))
                    {
                        ResolvedHosts.Add(host, ips = Dns.GetHostEntry(host).AddressList);
                    }
                }
                return ips
                    .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork || ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    .All(ip => IsLocalAddress(ip.ToString()));
            }
            return false;
        }

        public async Task<TunnelSocket> GetPveVNCWebSocket(string vmId)
        {
            var nodeId = Api.NodeId(vmId);
            //var vm = GetVirtualMachine(vmId);

            var vnc = Api.Nodes[nodeId.Node].Qemu[nodeId.Id].Vncproxy.Vncproxy().Result;
            var dic = vnc.ResponseToDictionary;
            var data = dic["data"] as IDictionary<string, object>;
            var ticket = WebUtility.UrlEncode(data["ticket"] as string);
            var port = int.Parse(data["port"] as string);

            await Api.Nodes[nodeId.Node].Qemu[nodeId.Id].Vncwebsocket.Vncwebsocket(port, ticket);

            //var url = $"https://{ProxmoxClusterServerHost}:{ProxmoxClusterServerPort}/?console=kvm&novnc=1&node={nodeId.Node}" +
            //    $"&resize=1&vmid={nodeId.Id}&path=api2/json/nodes/{nodeId.Node}/qemu/{nodeId.Id}/vncwebsocket/port/{port}/vncticket/{ticket}";
            var url = $"https://{ProxmoxClusterServerHost}:{ProxmoxClusterServerPort}/api2/json/nodes/{nodeId.Node}/qemu/{nodeId.Id}/vncwebsocket/port/{port}/vncticket/{ticket}";

            var tunnel = new TunnelSocket(url);
            tunnel.Cookies.Add(new Cookie("PVEAuthCookie", WebUtility.UrlEncode(Api.PVEAuthCookie) + ";SameSite=Strict;"));
            tunnel.HttpHeaders.Add("CSRFPreventionToken", Api.CSRFPreventionToken);

            return tunnel;
        }

        bool IsApiServerLoopback => DnsService.IsHostLoopback(ProxmoxClusterServerApiHost);
        bool IsApiIpV4 => DnsService.IsHostIpV4(ProxmoxClusterServerApiHost);
        #endregion


        #region Snapshots

        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            List<VirtualMachineSnapshot> snapshots = new List<VirtualMachineSnapshot>();
            try
            {
                String current_snapshot = "none";
                var vm = GetVirtualMachine(vmId);
                var RestResponse = Api.ListSnapshots(vmId);
                //JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(RestResponse.Content);
                //JsonArray jsonResponsearray = (JsonArray)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());
                JToken jsonResponse = JToken.Parse(RestResponse.Content);
                JArray jsonResponsearray = (JArray)jsonResponse["data"];

                foreach (JObject snapshot in jsonResponsearray)
                {
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
                var snapshot = Api.GetSnapshot(vmId, snapshotId);
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

        public string SnapshotScreenshotFile(string snapshotId) {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SolidCP", "Snapshots", $"proxmox-screenshot-{snapshotId}.png");
        }

        public JobResult CreateSnapshot(string vmId)
        {
            try
            {

                var vm = GetVirtualMachine(vmId);
                string snapname = $"wsp{vmId.Split(':')[1]}uid{Guid.NewGuid().ToString().GetHashCode().ToString("x")}";
                string snapdescr = $"{vm.Name} - {SNAPSHOT_DESCR} ({DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")})";

                // Save screenshot
                SKImage img = Api.GetScreenshot(vmId);
                if (img != null)
                {
                    var screenshotFile = SnapshotScreenshotFile(snapname);
                    var dir = Path.GetDirectoryName(screenshotFile);
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    using (var file = File.Create(screenshotFile))
                    using (var data = img.Encode())
                    {
                        data.SaveTo(file);
                    }
                }

                var snapshot = Api.CreateSnapshot(vmId, snapname, snapdescr);
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
                var renameresult = Api.RenameSnapshot(vmId, snapshotId, name);
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
                var applyresult = Api.Rollback(vmId, snapshotId);
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
            // Delete screenshot
            var snapshot = GetSnapshot(vmId, snapshotId);
            var screenshotFile = SnapshotScreenshotFile(snapshotId);
            if (File.Exists(screenshotFile)) File.Delete(screenshotFile);


            JobResult jobResult = ProxmoxJobHelper.CreateResult(ConcreteJobState.Running, ReturnCode.JobStarted);
            try
            {
                var deleteresult = Api.DeleteSnapshot(vmId, snapshotId);
                JToken jsonResponse = JToken.Parse(deleteresult.Content);
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

        public virtual Stream GetFile(string vmId, string filename, bool delete = false)
        {
            var ssh = SshClient();
            ssh.Connect();
            ssh.RunCommand($"sudo -n chown {DeploySSHUserSettings} {filename}");
            var enc = Encoding.GetEncoding("iso-8859-1");
            var cat = ssh.CreateCommand($"cat {filename}", enc);
            var result = cat.Execute();
            var mem = new MemoryStream(enc.GetBytes(result));
            if (delete) ssh.RunCommand($"rm {filename}");
            return mem;
        }

        public ImageFile GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
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

            var screenshotFile = SnapshotScreenshotFile(snapshotId);
            if (File.Exists(screenshotFile))
            {
                var img = SKImage.FromEncodedData(screenshotFile);
                SKImageInfo thumbinfo = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Opaque);
                SKImage thumb = SKImage.Create(thumbinfo);
                img?.ScalePixels(thumb.PeekPixels(), SKFilterQuality.Medium);

                MemoryStream stream = new MemoryStream();

                using (var data = thumb.Encode(SKEncodedImageFormat.Png, 100))
                {
                    data.SaveTo(stream);
                }
                stream.Flush();
                var buffer = stream.ToArray();

                return new ImageFile()
                {
                    FileExtension = "png",
                    MimeType = "image/png",
                    RawData = buffer
                };
            }
            else
            {
                var assembly = Assembly.GetExecutingAssembly();
                var svgName = assembly.GetManifestResourceNames()
                    .First(name => name.EndsWith("stop-sign-svgrepo-com.svg", StringComparison.OrdinalIgnoreCase));
                using (var res = assembly.GetManifestResourceStream(svgName))
                {
                    var bytes = new byte[res.Length];
                    res.Read(bytes, 0, (int)res.Length);
                    return new ImageFile()
                    {
                        MimeType = "image/svg+xml",
                        FileExtension = "svg",
                        RawData = bytes
                    };
                }
            }
        }

        #endregion

        #region DVD operations
        public LibraryItem[] GetDVDISOs(string vmId)
        {
            List<LibraryItem> imagelist = new List<LibraryItem>();
            try
            {
                var RestResponse = Api.ListISOs(vmId, ProxmoxIsosonStorage);
                JToken jsonResponse = JToken.Parse(RestResponse.Content);
                JArray jsonResponsearray = (JArray)jsonResponse["data"];

                foreach (JObject dvd in jsonResponsearray)
                {
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
                            item.DiskSize = Int64.Parse(dvd["size"].ToString());
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

            DvdDriveInfo dvdInfo;
            try
            {
                dvdInfo = ProxmoxDvdDriveHelper.Get(Api, vmId);
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

            long disksize = 0;

            // load library items
            LibraryItem[] disks = GetDVDISOs(vmId);

            // find required disk
            foreach (LibraryItem disk in disks)
            {
                if (string.Compare(isoPath, disk.Path, true) == 0)
                    disksize = disk.DiskSize;
            }

            try
            {
                ProxmoxDvdDriveHelper.Set(Api, vmId, isoPath, disksize);
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

            try
            {
                ProxmoxDvdDriveHelper.Set(Api, vmId, null, 0);
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


        public virtual string ReadRemoteFile(string path)
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

        public virtual void WriteRemoteFile(string path, string content)
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
            var taskstatus = Api.TaskStatus(jobId);
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

                var RestResponse = Api.ClusterResources();
                //JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(RestResponse.Content);
                //JsonArray jsonResponsearray = (JsonArray)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());
                JToken jsonResponse = JToken.Parse(RestResponse.Content);
                JArray jsonResponsearray = (JArray)jsonResponse["data"];

                foreach (JObject resources in jsonResponsearray)
                {
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

        User user = null;
        public User User
        {
            get
            {
                if (user != null) return user;

                string clusterrealm = PROXMOX_REALM;
                if (ProxmoxClusterRealm != null && ProxmoxClusterRealm != "")
                    clusterrealm = ProxmoxClusterRealm;

                return user = new User { Username = ProxmoxClusterAdminUser, Password = ProxmoxClusterAdminPass, Realm = clusterrealm };
            }
        }
        #endregion


        #region Linux to Windows States & Formats
        private VirtualMachineState GetVMState(string state)
        {
            HostedSolutionLog.LogInfo("getvmstate - state: {0}", state);
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

        bool isDisposed = false;
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                Api.Dispose();
            }
        }

        #endregion

    }
}