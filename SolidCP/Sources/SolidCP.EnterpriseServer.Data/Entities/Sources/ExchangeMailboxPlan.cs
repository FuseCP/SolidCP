// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities.Sources;

#if NetCore
[Index("ItemId", Name = "ExchangeMailboxPlansIdx_ItemID")]
[Index("MailboxPlanId", Name = "IX_ExchangeMailboxPlans", IsUnique = true)]
#endif
public partial class ExchangeMailboxPlan
{
    [Key]
    public int MailboxPlanId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [StringLength(300)]
    public string MailboxPlan { get; set; }

    public int? MailboxPlanType { get; set; }

    public bool EnableActiveSync { get; set; }

    [Column("EnableIMAP")]
    public bool EnableImap { get; set; }

    [Column("EnableMAPI")]
    public bool EnableMapi { get; set; }

    [Column("EnableOWA")]
    public bool EnableOwa { get; set; }

    [Column("EnablePOP")]
    public bool EnablePop { get; set; }

    public bool IsDefault { get; set; }

    public int IssueWarningPct { get; set; }

    public int KeepDeletedItemsDays { get; set; }

    [Column("MailboxSizeMB")]
    public int MailboxSizeMb { get; set; }

    [Column("MaxReceiveMessageSizeKB")]
    public int MaxReceiveMessageSizeKb { get; set; }

    public int MaxRecipients { get; set; }

    [Column("MaxSendMessageSizeKB")]
    public int MaxSendMessageSizeKb { get; set; }

    public int ProhibitSendPct { get; set; }

    public int ProhibitSendReceivePct { get; set; }

    public bool HideFromAddressBook { get; set; }

    public bool? AllowLitigationHold { get; set; }

    public int? RecoverableItemsWarningPct { get; set; }

    public int? RecoverableItemsSpace { get; set; }

    [StringLength(256)]
    public string LitigationHoldUrl { get; set; }

    [StringLength(512)]
    public string LitigationHoldMsg { get; set; }

    public bool? Archiving { get; set; }

    public bool? EnableArchiving { get; set; }

    [Column("ArchiveSizeMB")]
    public int? ArchiveSizeMb { get; set; }

    public int? ArchiveWarningPct { get; set; }

    public bool? EnableAutoReply { get; set; }

    public bool? IsForJournaling { get; set; }

    public bool? EnableForceArchiveDeletion { get; set; }

    [InverseProperty("MailboxPlan")]
    public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();

    [ForeignKey("ItemId")]
    [InverseProperty("ExchangeMailboxPlans")]
    public virtual ExchangeOrganization Item { get; set; }
}
#endif