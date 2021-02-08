// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
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


namespace SolidCP.Providers.Virtualization
{
    public class VirtualMachine : ServiceProviderItem
    {
        public VirtualMachine()
        {
        }

        // properties
        [Persistent]
        public string VirtualMachineId { get; set; }
        public string Hostname { get; set; }
        public string Domain { get; set; }
        [Persistent]
        public string Version { get; set; }

        public VirtualMachineState State { get; set; }
        public long Uptime { get; set; }
        public OperationalStatus Heartbeat { get; set; }

        [Persistent]
        public string CreationTime { get; set; }
        [Persistent]
        public string RootFolderPath { get; set; }
        [Persistent]
        public string[] VirtualHardDrivePath { get; set; }
        [Persistent]
        public string OperatingSystemTemplate { get; set; }
        [Persistent]
        public string OperatingSystemTemplatePath { get; set; }
        [Persistent]
        public string OperatingSystemTemplateDeployParams { get; set; }
        [Persistent]
        public string AdministratorPassword { get; set; }

        [Persistent]
        public string CurrentTaskId { get; set; }
        [Persistent]
        public VirtualMachineProvisioningStatus ProvisioningStatus { get; set; }


        [Persistent]
        public int CpuCores { get; set; }
        public int CpuUsage { get; set; }

        [Persistent]
        public int RamSize { get; set; }
        public int RamUsage { get; set; }

        [Persistent]
        public DynamicMemory DynamicMemory { get; set; }

        [Persistent]
        public int[] HddSize { get; set; }
        public LogicalDisk[] HddLogicalDisks { get; set; }
        [Persistent]
        public int HddMaximumIOPS { get; set; }
        [Persistent]
        public int HddMinimumIOPS { get; set; }
        [Persistent]
        public int SnapshotsNumber { get; set; }

        [Persistent]
        public bool DvdDriveInstalled { get; set; }
        [Persistent]
        public bool BootFromCD { get; set; }
        [Persistent]
        public bool NumLockEnabled { get; set; }

        [Persistent]
        public bool StartTurnOffAllowed { get; set; }
        [Persistent]
        public bool PauseResumeAllowed { get; set; }
        [Persistent]
        public bool RebootAllowed { get; set; }
        [Persistent]
        public bool ResetAllowed { get; set; }
        [Persistent]
        public bool ReinstallAllowed { get; set; }

        [Persistent]
        public bool LegacyNetworkAdapter { get; set; }
        [Persistent]
        public bool RemoteDesktopEnabled { get; set; }

        [Persistent]
        public bool ExternalNetworkEnabled { get; set; }
        [Persistent]
        public string ExternalNicMacAddress { get; set; }
        [Persistent]
        public string ExternalSwitchId { get; set; }

        [Persistent]
        public bool PrivateNetworkEnabled { get; set; }
        [Persistent]
        public string PrivateNicMacAddress { get; set; }
        [Persistent]
        public string PrivateSwitchId { get; set; }
        [Persistent]
        public int PrivateNetworkVlan { get; set; }

        [Persistent]
        public bool ManagementNetworkEnabled { get; set; }
        [Persistent]
        public string ManagementNicMacAddress { get; set; }
        [Persistent]
        public string ManagementSwitchId { get; set; }

        // for GetVirtualMachineEx used in import method
        public VirtualMachineNetworkAdapter[] Adapters { get; set; }

        [Persistent]
        public VirtualHardDiskInfo[] Disks { get; set; }

        [Persistent]
        public string Status { get; set; }

        public ReplicationState ReplicationState { get; set; }

        [Persistent]
        public int Generation { get; set; }
        [Persistent]
        public string SecureBootTemplate { get; set; }
        [Persistent]
        public bool EnableSecureBoot { get; set; }

        [Persistent]
        public int ProcessorCount { get; set; }

        [Persistent]
        public string ParentSnapshotId { get; set; }
        [Persistent]
        public int defaultaccessvlan { get; set; }//external network vlan
        public VirtualMachineIPAddress PrimaryIP { get; set; }
        public bool NeedReboot { get; set; } //give access to force reboot a server.
        [Persistent]
        public string CustomPrivateGateway { get; set; }
        [Persistent]
        public string CustomPrivateDNS1 { get; set; }
        [Persistent]
        public string CustomPrivateDNS2 { get; set; }
        [Persistent]
        public string CustomPrivateMask { get; set; }
        [Persistent]
        public string ClusterName { get; set; }
        public bool IsClustered { get; set; }
    }
}
