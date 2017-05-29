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
using System.Text;

namespace SolidCP.EnterpriseServer
{
    [Serializable]
    public class GlobalDnsRecord
    {
        private int recordId;
        private string internalIP;
        private string externalIP;
        private int recordOrder;
        private int groupId;
        private int serviceId;
        private int serverId;
        private int packageId;
        private string recordType;
        private string recordName;
        private string recordData;
        private int mxPriority;
        private int ipAddressId;
        private int srvPriority;
        private int srvWeight;
        private int srvPort;


        public int RecordId
        {
            get { return recordId; }
            set { recordId = value; }
        }


        public int RecordOrder
        {
            get { return recordOrder; }
            set { recordOrder = value; }
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public int ServiceId
        {
            get { return serviceId; }
            set { serviceId = value; }
        }

        public int ServerId
        {
            get { return serverId; }
            set { serverId = value; }
        }

        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }

        public string RecordType
        {
            get { return recordType; }
            set { recordType = value; }
        }

        public string RecordName
        {
            get { return recordName; }
            set { recordName = value; }
        }

        public string RecordData
        {
            get { return recordData; }
            set { recordData = value; }
        }

        public int MxPriority
        {
            get { return mxPriority; }
            set { mxPriority = value; }
        }


        public int IpAddressId
        {
            get { return ipAddressId; }
            set { ipAddressId = value; }
        }

        public GlobalDnsRecord()
        {
        }

        public string InternalIP
        {
            get { return this.internalIP; }
            set { this.internalIP = value; }
        }

        public string ExternalIP
        {
            get { return this.externalIP; }
            set { this.externalIP = value; }
        }


        public int SrvPriority
        {
            get { return srvPriority; }
            set { srvPriority = value; }
        }

        public int SrvWeight
        {
            get { return srvWeight; }
            set { srvWeight = value; }
        }

        public int SrvPort
        {
            get { return srvPort; }
            set { srvPort = value; }
        }
    }
}
