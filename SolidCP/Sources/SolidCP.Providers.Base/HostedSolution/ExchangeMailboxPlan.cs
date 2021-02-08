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
using System.Text.RegularExpressions;

namespace SolidCP.Providers.HostedSolution
{
    [Serializable]
    public class ExchangeMailboxPlan
    {
        int itemId;
        int mailboxPlanId;
        string mailboxPlan;

        public override string ToString()
        {
            if (mailboxPlan != null)
                return mailboxPlan;

            return base.ToString();
        }

        int mailboxSizeMB;
        int maxRecipients;
        int maxSendMessageSizeKB;
        int maxReceiveMessageSizeKB;
        bool enableAutoReply;

        bool enablePOP;
        bool enableIMAP;
        bool enableOWA;
        bool enableMAPI;
        bool enableActiveSync;

        int issueWarningPct;
        int prohibitSendPct;
        int prohibitSendReceivePct;
        int keepDeletedItemsDays;
        bool isDefault;
        bool hideFromAddressBook;
        int mailboxPlanType;

        bool allowLitigationHold;
	    int recoverableItemsWarningPct;
        int recoverableItemsSpace;
        string litigationHoldUrl;
        string litigationHoldMsg;

        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }

        [LogProperty("Mailbox Plan ID")]
        public int MailboxPlanId
        {
            get { return this.mailboxPlanId; }
            set { this.mailboxPlanId = value; }
        }

        [LogProperty("Mailbox Plan Name")]
        public string MailboxPlan
        {
            get { return this.mailboxPlan; }
            set { this.mailboxPlan = value; }
        }

        public int MailboxPlanType
        {
            get { return this.mailboxPlanType; }
            set { this.mailboxPlanType = value; }
        }


        public int MailboxSizeMB
        {
            get { return this.mailboxSizeMB; }
            set { this.mailboxSizeMB = value; }
        }

        public bool IsDefault
        {
            get { return this.isDefault; }
            set { this.isDefault = value; }
        }

        public int MaxRecipients
        {
            get { return this.maxRecipients; }
            set { this.maxRecipients = value; }
        }

        public int MaxSendMessageSizeKB
        {
            get { return this.maxSendMessageSizeKB; }
            set { this.maxSendMessageSizeKB = value; }
        }

        public int MaxReceiveMessageSizeKB
        {
            get { return this.maxReceiveMessageSizeKB; }
            set { this.maxReceiveMessageSizeKB = value; }
        }

        public bool EnableAutoReply
        {
            get { return this.enableAutoReply; }
            set { this.enableAutoReply = value; }
        }

        public bool EnablePOP
        {
            get { return this.enablePOP; }
            set { this.enablePOP = value; }
        }

        public bool EnableIMAP
        {
            get { return this.enableIMAP; }
            set { this.enableIMAP = value; }
        }

        public bool EnableOWA
        {
            get { return this.enableOWA; }
            set { this.enableOWA = value; }
        }

        public bool EnableMAPI
        {
            get { return this.enableMAPI; }
            set { this.enableMAPI = value; }
        }

        public bool EnableActiveSync
        {
            get { return this.enableActiveSync; }
            set { this.enableActiveSync = value; }
        }

        public int IssueWarningPct
        {
            get { return this.issueWarningPct; }
            set { this.issueWarningPct = value; }
        }

        public int ProhibitSendPct
        {
            get { return this.prohibitSendPct; }
            set { this.prohibitSendPct = value; }
        }

        public int ProhibitSendReceivePct
        {
            get { return this.prohibitSendReceivePct; }
            set { this.prohibitSendReceivePct = value; }
        }

        public int KeepDeletedItemsDays
        {
            get { return this.keepDeletedItemsDays; }
            set { this.keepDeletedItemsDays = value; }
        }

        public bool HideFromAddressBook
        {
            get { return this.hideFromAddressBook; }
            set { this.hideFromAddressBook = value; }
        }


        public bool AllowLitigationHold
        {
            get { return this.allowLitigationHold; }
            set { this.allowLitigationHold = value; }
        }

        public int RecoverableItemsWarningPct
        {
            get { return this.recoverableItemsWarningPct; }
            set { this.recoverableItemsWarningPct = value; }
        }

        public int RecoverableItemsSpace
        {
            get { return this.recoverableItemsSpace; }
            set { this.recoverableItemsSpace = value; }
        }

        public string LitigationHoldUrl
        {
            get { return this.litigationHoldUrl; }
            set { this.litigationHoldUrl = value; }
        }

        public string LitigationHoldMsg
        {
            get { return this.litigationHoldMsg; }
            set { this.litigationHoldMsg = value; }
        }

        bool archiving;
        public bool Archiving
        {
            get { return this.archiving; }
            set { this.archiving = value; }
        }

        bool enableArchiving;
        public bool EnableArchiving
        {
            get { return this.enableArchiving; }
            set { this.enableArchiving = value; }
        }

        int archiveSizeMB;
        public int ArchiveSizeMB
        {
            get { return this.archiveSizeMB; }
            set { this.archiveSizeMB = value; }
        }

        int archiveWarningPct;
        public int ArchiveWarningPct
        {
            get { return this.archiveWarningPct; }
            set { this.archiveWarningPct = value; }
        }

        bool enableForceArchiveDeletion;
        public bool EnableForceArchiveDeletion
        {
            get { return this.enableForceArchiveDeletion; }
            set { this.enableForceArchiveDeletion = value; }
        }

        bool isForJournaling;
        public bool IsForJournaling
        {
            get { return this.isForJournaling; }
            set { this.isForJournaling = value; }
        }

        [LogProperty("Mailbox Plan Unique Name")]
        public string SCPUniqueName
        {
            get
            {
                Regex r = new Regex(@"[^A-Za-z0-9]");
                return "SCPPolicy" + MailboxPlanId.ToString() + "_" + r.Replace(MailboxPlan, "");
            }
        }
    }
}
