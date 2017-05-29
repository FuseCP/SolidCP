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
    public class HostingPlanInfo
    {
        int planId;
        int userId;
        int packageId;
        int serverId;
        string planName;
        string planDescription;
        bool available;
        bool isAddon;
        decimal setupPrice;
        decimal recurringPrice;
        int recurrenceUnit;
        int recurrenceLength;
        HostingPlanGroupInfo[] groups;
        HostingPlanQuotaInfo[] quotas;

        public int PlanId
        {
            get { return planId; }
            set { planId = value; }
        }

        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }

        public string PlanName
        {
            get { return planName; }
            set { planName = value; }
        }

        public string PlanDescription
        {
            get { return planDescription; }
            set { planDescription = value; }
        }

        public bool Available
        {
            get { return available; }
            set { available = value; }
        }

        public int ServerId
        {
            get { return serverId; }
            set { serverId = value; }
        }

        public bool IsAddon
        {
            get { return isAddon; }
            set { isAddon = value; }
        }

        public decimal SetupPrice
        {
            get { return this.setupPrice; }
            set { this.setupPrice = value; }
        }

        public decimal RecurringPrice
        {
            get { return this.recurringPrice; }
            set { this.recurringPrice = value; }
        }

        public int RecurrenceUnit
        {
            get { return this.recurrenceUnit; }
            set { this.recurrenceUnit = value; }
        }

        public int RecurrenceLength
        {
            get { return this.recurrenceLength; }
            set { this.recurrenceLength = value; }
        }

        public HostingPlanGroupInfo[] Groups
        {
            get { return this.groups; }
            set { this.groups = value; }
        }

        public HostingPlanQuotaInfo[] Quotas
        {
            get { return this.quotas; }
            set { this.quotas = value; }
        }

        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }
    }
}
