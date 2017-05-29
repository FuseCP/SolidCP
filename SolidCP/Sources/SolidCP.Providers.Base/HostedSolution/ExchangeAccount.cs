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

namespace SolidCP.Providers.HostedSolution
{
    public class ExchangeAccount
    {
        int accountId;
        int itemId;
        int packageId;
        string subscriberNumber;
        ExchangeAccountType accountType;
        string accountName;
        string displayName;
        string primaryEmailAddress;
        bool mailEnabledPublicFolder;
        MailboxManagerActions mailboxManagerActions;
        string accountPassword;
        string samAccountName;
        int mailboxPlanId;
        string mailboxPlan;
        string publicFolderPermission;
        string userPrincipalName;
        string notes;
        int levelId;
        bool isVip;

        [LogProperty]
        public int AccountId
        {
            get { return this.accountId; }
            set { this.accountId = value; }
        }

        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }

        public int PackageId
        {
            get { return this.packageId; }
            set { this.packageId = value; }
        }

        public ExchangeAccountType AccountType
        {
            get { return this.accountType; }
            set { this.accountType = value; }
        }

        [LogProperty]
        public string AccountName
        {
            get { return this.accountName; }
            set { this.accountName = value; }
        }

        [LogProperty]
        public string SamAccountName
        {
            get { return this.samAccountName; }
            set { this.samAccountName = value; }
        }

        [LogProperty]
        public string DisplayName
        {
            get { return this.displayName; }
            set { this.displayName = value; }
        }

        [LogProperty("Email Address")]
        public string PrimaryEmailAddress
        {
            get { return this.primaryEmailAddress; }
            set { this.primaryEmailAddress = value; }
        }

        public bool MailEnabledPublicFolder
        {
            get { return this.mailEnabledPublicFolder; }
            set { this.mailEnabledPublicFolder = value; }
        }

        //public string AccountPassword
        //{
        //    get { return this.accountPassword; }
        //    set { this.accountPassword = value; }
        //}

        public MailboxManagerActions MailboxManagerActions
        {
            get { return this.mailboxManagerActions; }
            set { this.mailboxManagerActions = value; }
        }

        public int MailboxPlanId
        {
            get { return this.mailboxPlanId; }
            set { this.mailboxPlanId = value; }
        }

        public string MailboxPlan
        {
            get { return this.mailboxPlan; }
            set { this.mailboxPlan = value; }
        }


        public string SubscriberNumber
        {
            get { return this.subscriberNumber; }
            set { this.subscriberNumber = value; }
        }

        public string PublicFolderPermission
        {
            get { return this.publicFolderPermission; }
            set { this.publicFolderPermission = value; }
        }


        public string UserPrincipalName
        {
            get { return this.userPrincipalName; }
            set { this.userPrincipalName = value; }
        }

        public string Notes
        {
            get { return this.notes; }
            set { this.notes = value; }
        }

        int archivingMailboxPlanId;
        public int ArchivingMailboxPlanId
        {
            get { return this.archivingMailboxPlanId; }
            set { this.archivingMailboxPlanId = value; }
        }

        string archivingMailboxPlan;
        public string ArchivingMailboxPlan
        {
            get { return this.archivingMailboxPlan; }
            set { this.archivingMailboxPlan = value; }
        }

        bool enableArchiving;
        public bool EnableArchiving
        {
            get { return this.enableArchiving; }
            set { this.enableArchiving = value; }
        }

        public bool IsVIP
        {
            get { return this.isVip; }
            set { this.isVip = value; }
        }

        public int LevelId
        {
            get { return this.levelId; }
            set { this.levelId = value; }
        }

        public bool Disabled { get; set; }

        public bool Locked { get; set; }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(accountName) ? accountName : base.ToString();
        }
    }
}
