#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ServiceItem
{
    public int ItemId { get; set; }

    public int? PackageId { get; set; }

    public int? ItemTypeId { get; set; }

    public int? ServiceId { get; set; }

    public string ItemName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Domain> DomainMailDomains { get; set; } = new List<Domain>();

    public virtual ICollection<Domain> DomainWebSites { get; set; } = new List<Domain>();

    public virtual ICollection<Domain> DomainZoneItems { get; set; } = new List<Domain>();

    public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();

    public virtual ExchangeOrganization ExchangeOrganization { get; set; }

    public virtual ICollection<ExchangeOrganizationDomain> ExchangeOrganizationDomains { get; set; } = new List<ExchangeOrganizationDomain>();

    public virtual ServiceItemType ItemType { get; set; }

    public virtual Package Package { get; set; }

    public virtual ICollection<PackageIpaddress> PackageIpaddresses { get; set; } = new List<PackageIpaddress>();

    public virtual ICollection<PrivateIpaddress> PrivateIpaddresses { get; set; } = new List<PrivateIpaddress>();

    public virtual Service Service { get; set; }

    public virtual ICollection<ServiceItemProperty> ServiceItemProperties { get; set; } = new List<ServiceItemProperty>();
}
#endif