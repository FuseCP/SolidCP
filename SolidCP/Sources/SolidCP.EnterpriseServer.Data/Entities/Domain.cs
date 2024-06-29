#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

#if NetCore
[Index("MailDomainId", Name = "DomainsIdx_MailDomainID")]
[Index("PackageId", Name = "DomainsIdx_PackageID")]
[Index("WebSiteId", Name = "DomainsIdx_WebSiteID")]
[Index("ZoneItemId", Name = "DomainsIdx_ZoneItemID")]
#endif
public partial class Domain
{
    [Key]
    [Column("DomainID")]
    public int DomainId { get; set; }

    [Column("PackageID")]
    public int PackageId { get; set; }

    [Column("ZoneItemID")]
    public int? ZoneItemId { get; set; }

    [Required]
    [StringLength(100)]
    public string DomainName { get; set; }

    public bool HostingAllowed { get; set; }

    [Column("WebSiteID")]
    public int? WebSiteId { get; set; }

    [Column("MailDomainID")]
    public int? MailDomainId { get; set; }

    public bool IsSubDomain { get; set; }

    public bool IsPreviewDomain { get; set; }

    public bool IsDomainPointer { get; set; }

    public int? DomainItemId { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? ExpirationDate { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? LastUpdateDate { get; set; }

    public string RegistrarName { get; set; }

    [InverseProperty("Domain")]
    public virtual ICollection<DomainDnsRecord> DomainDnsRecords { get; set; } = new List<DomainDnsRecord>();

    [ForeignKey("MailDomainId")]
    [InverseProperty("DomainMailDomains")]
    public virtual ServiceItem MailDomain { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("Domains")]
    public virtual Package Package { get; set; }

    [ForeignKey("WebSiteId")]
    [InverseProperty("DomainWebSites")]
    public virtual ServiceItem WebSite { get; set; }

    [ForeignKey("ZoneItemId")]
    [InverseProperty("DomainZones")]
    public virtual ServiceItem Zone { get; set; }
}
#endif