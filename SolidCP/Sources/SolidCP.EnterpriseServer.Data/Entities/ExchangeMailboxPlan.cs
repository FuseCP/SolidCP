// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeMailboxPlan
{
    public int MailboxPlanId { get; set; }

    public int ItemId { get; set; }

    public string MailboxPlan { get; set; }

    public int? MailboxPlanType { get; set; }

    public bool EnableActiveSync { get; set; }

    public bool EnableImap { get; set; }

    public bool EnableMapi { get; set; }

    public bool EnableOwa { get; set; }

    public bool EnablePop { get; set; }

    public bool IsDefault { get; set; }

    public int IssueWarningPct { get; set; }

    public int KeepDeletedItemsDays { get; set; }

    public int MailboxSizeMb { get; set; }

    public int MaxReceiveMessageSizeKb { get; set; }

    public int MaxRecipients { get; set; }

    public int MaxSendMessageSizeKb { get; set; }

    public int ProhibitSendPct { get; set; }

    public int ProhibitSendReceivePct { get; set; }

    public bool HideFromAddressBook { get; set; }

    public bool? AllowLitigationHold { get; set; }

    public int? RecoverableItemsWarningPct { get; set; }

    public int? RecoverableItemsSpace { get; set; }

    public string LitigationHoldUrl { get; set; }

    public string LitigationHoldMsg { get; set; }

    public bool? Archiving { get; set; }

    public bool? EnableArchiving { get; set; }

    public int? ArchiveSizeMb { get; set; }

    public int? ArchiveWarningPct { get; set; }

    public bool? EnableAutoReply { get; set; }

    public bool? IsForJournaling { get; set; }

    public bool? EnableForceArchiveDeletion { get; set; }

    public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();

    public virtual ExchangeOrganization Item { get; set; }
}
#endif