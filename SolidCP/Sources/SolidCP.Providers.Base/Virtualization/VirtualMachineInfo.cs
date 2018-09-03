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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.Providers.Virtualization
{
    public class VMInfo : ServiceProviderItem
    {
        [Persistent]
        public  int VmId {get; set; }

        [Persistent]
        public Guid VmGuid { get; set; }

        [Persistent]
        public string CreationTime { get; set; }
        [Persistent]
        public Guid TemplateId { get; set; }
        [Persistent]
        public string TemplateName { get; set; }
        [Persistent]
        public string CurrentTaskId { get; set; }
        [Persistent]
        public string VmPath { get; set; }
        [Persistent]
        public string ProductKey { get; set; }
        [Persistent]
        public string HostName { get; set; }
        [Persistent]
        public string ComputerName { get; set; }
        [Persistent]
        public string Owner { get; set; }
        [Persistent]
        public string AdminUserName { get; set; }
        [Persistent]
        public string AdminPassword { get; set; }
        public string JoinDomain { get; set; }
        public string JoinDomainUserName { get; set; }
        public string JoinDomainPassword { get; set; }
        public bool MergeAnswerFile { get; set; }
        [Persistent]
        public int Generation { get; set; }
        [Persistent]
        public int CPUCount { get; set; }
        [Persistent]
        public int CPUUtilization { get; set; }
        public int PerfCPUUtilization { get; set; }

        [Persistent]
        public int Memory { get; set; }

        public int ProcessMemory { get; set; }

        [Persistent]
        public bool NumLockEnabled { get; set; }
        [Persistent]
        public bool DvdDriver { get; set; }
        [Persistent]
        public int HddSize { get; set; }
        [Persistent]
        public int SnapshotsNumber { get; set; }
        [Persistent]
        public int HddMaximumIOPS { get; set; }
        [Persistent]
        public int HddMinimumIOPS { get; set; }
        [Persistent]
        public bool BootFromCD { get; set; }

        public LogicalDisk[] HddLogicalDisks { get; set; }

        [Persistent]
        public bool ExternalNetworkEnabled { get; set; }
        [Persistent]
        public string ExternalNicMacAddress { get; set; }
        [Persistent]
        public string ExternalVirtualNetwork { get; set; }
        [Persistent]
        public string ExternalNetworkLocation { get; set; }

        [Persistent]
        public bool PrivateNetworkEnabled { get; set; }
        [Persistent]
        public string PrivateNicMacAddress { get; set; }
        [Persistent]
        public string PrivateVirtualNetwork { get; set; }
        [Persistent]
        public ushort PrivateVLanID { get; set; }
        [Persistent]
        public string PrivateNetworkLocation { get; set; }

        [Persistent]
        public bool ManagementNetworkEnabled { get; set; }
        [Persistent]
        public string ManagementNicMacAddress { get; set; }

        [Persistent]
        public VirtualMachineProvisioningStatus ProvisioningStatus { get; set; }
        [Persistent]
        public VMComputerSystemStateInfo State { get; set; }

        public string ModifiedTime { get; set; }

        // User configuration
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
        public bool EnableSecureBoot { get; set; }

        [Persistent]
        public ConcreteJob CurrentJob { get; set; }

        public string exMessage { get; set; }

        public string logMessage { get; set; }
    }
}
