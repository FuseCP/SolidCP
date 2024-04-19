// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeAccount
{
    public int AccountId { get; set; }

    public int ItemId { get; set; }

    public int AccountType { get; set; }

    public string AccountName { get; set; }

    public string DisplayName { get; set; }

    public string PrimaryEmailAddress { get; set; }

    public bool? MailEnabledPublicFolder { get; set; }

    public string MailboxManagerActions { get; set; }

    public string SamAccountName { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? MailboxPlanId { get; set; }

    public string SubscriberNumber { get; set; }

    public string UserPrincipalName { get; set; }

    public int? ExchangeDisclaimerId { get; set; }

    public int? ArchivingMailboxPlanId { get; set; }

    public bool? EnableArchiving { get; set; }

    public int? LevelId { get; set; }

    public bool IsVip { get; set; }

    public virtual ICollection<AccessToken> AccessTokens { get; set; } = new List<AccessToken>();

    public virtual ICollection<BlackBerryUser> BlackBerryUsers { get; set; } = new List<BlackBerryUser>();

    public virtual ICollection<Crmuser> Crmusers { get; set; } = new List<Crmuser>();

    public virtual ICollection<EnterpriseFoldersOwaPermission> EnterpriseFoldersOwaPermissions { get; set; } = new List<EnterpriseFoldersOwaPermission>();

    public virtual ICollection<ExchangeAccountEmailAddress> ExchangeAccountEmailAddresses { get; set; } = new List<ExchangeAccountEmailAddress>();

    public virtual ServiceItem Item { get; set; }

    public virtual ExchangeMailboxPlan MailboxPlan { get; set; }

    public virtual ICollection<RdscollectionUser> RdscollectionUsers { get; set; } = new List<RdscollectionUser>();

    public virtual ICollection<WebDavAccessToken> WebDavAccessTokens { get; set; } = new List<WebDavAccessToken>();

    public virtual ICollection<WebDavPortalUsersSetting> WebDavPortalUsersSettings { get; set; } = new List<WebDavPortalUsersSetting>();
}
#endif