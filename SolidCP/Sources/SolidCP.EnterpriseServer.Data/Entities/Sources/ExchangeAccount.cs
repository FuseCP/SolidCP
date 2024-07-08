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
[Index("ItemId", Name = "ExchangeAccountsIdx_ItemID")]
[Index("MailboxPlanId", Name = "ExchangeAccountsIdx_MailboxPlanId")]
[Index("AccountName", Name = "IX_ExchangeAccounts_UniqueAccountName", IsUnique = true)]
#endif
public partial class ExchangeAccount
{
    [Key]
    [Column("AccountID")]
    public int AccountId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    public int AccountType { get; set; }

    [Required]
    [StringLength(300)]
    public string AccountName { get; set; }

    [Required]
    [StringLength(300)]
    public string DisplayName { get; set; }

    [StringLength(300)]
    public string PrimaryEmailAddress { get; set; }

    public bool? MailEnabledPublicFolder { get; set; }

    [StringLength(200)]
#if NetCore
    [Unicode(false)]
#endif
    public string MailboxManagerActions { get; set; }

    [StringLength(100)]
    public string SamAccountName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public int? MailboxPlanId { get; set; }

    [StringLength(32)]
    public string SubscriberNumber { get; set; }

    [StringLength(300)]
    public string UserPrincipalName { get; set; }

    public int? ExchangeDisclaimerId { get; set; }

    public int? ArchivingMailboxPlanId { get; set; }

    public bool? EnableArchiving { get; set; }

    [Column("LevelID")]
    public int? LevelId { get; set; }

    [Column("IsVIP")]
    public bool IsVip { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<AccessToken> AccessTokens { get; set; } = new List<AccessToken>();

    [InverseProperty("Account")]
    public virtual ICollection<BlackBerryUser> BlackBerryUsers { get; set; } = new List<BlackBerryUser>();

    [InverseProperty("Account")]
    public virtual ICollection<Crmuser> Crmusers { get; set; } = new List<Crmuser>();

    [InverseProperty("Account")]
    public virtual ICollection<EnterpriseFoldersOwaPermission> EnterpriseFoldersOwaPermissions { get; set; } = new List<EnterpriseFoldersOwaPermission>();

    [InverseProperty("Account")]
    public virtual ICollection<ExchangeAccountEmailAddress> ExchangeAccountEmailAddresses { get; set; } = new List<ExchangeAccountEmailAddress>();

    [ForeignKey("ItemId")]
    [InverseProperty("ExchangeAccounts")]
    public virtual ServiceItem Item { get; set; }

    [ForeignKey("MailboxPlanId")]
    [InverseProperty("ExchangeAccounts")]
    public virtual ExchangeMailboxPlan MailboxPlan { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<RdscollectionUser> RdscollectionUsers { get; set; } = new List<RdscollectionUser>();

    [InverseProperty("Account")]
    public virtual ICollection<WebDavAccessToken> WebDavAccessTokens { get; set; } = new List<WebDavAccessToken>();

    [InverseProperty("Account")]
    public virtual ICollection<WebDavPortalUsersSetting> WebDavPortalUsersSettings { get; set; } = new List<WebDavPortalUsersSetting>();
}
#endif