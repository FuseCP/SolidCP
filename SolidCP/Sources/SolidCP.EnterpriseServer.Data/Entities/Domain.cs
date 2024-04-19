// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Domain
{
    public int DomainId { get; set; }

    public int PackageId { get; set; }

    public int? ZoneItemId { get; set; }

    public string DomainName { get; set; }

    public bool HostingAllowed { get; set; }

    public int? WebSiteId { get; set; }

    public int? MailDomainId { get; set; }

    public bool IsSubDomain { get; set; }

    public bool IsPreviewDomain { get; set; }

    public bool IsDomainPointer { get; set; }

    public int? DomainItemId { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public string RegistrarName { get; set; }

    public virtual ICollection<DomainDnsRecord> DomainDnsRecords { get; set; } = new List<DomainDnsRecord>();

    public virtual ServiceItem MailDomain { get; set; }

    public virtual Package Package { get; set; }

    public virtual ServiceItem WebSite { get; set; }

    public virtual ServiceItem ZoneItem { get; set; }
}
#endif