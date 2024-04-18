#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeOrganizationDomain
{
    public int OrganizationDomainId { get; set; }

    public int ItemId { get; set; }

    public int? DomainId { get; set; }

    public bool? IsHost { get; set; }

    public int DomainTypeId { get; set; }

    public virtual ServiceItem Item { get; set; }
}
#endif