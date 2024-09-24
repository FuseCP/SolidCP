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
[Index("ClusterId", Name = "ServicesIdx_ClusterID")]
[Index("ProviderId", Name = "ServicesIdx_ProviderID")]
[Index("ServerId", Name = "ServicesIdx_ServerID")]
#endif
public partial class Service
{
    [Key]
    [Column("ServiceID")]
    public int ServiceId { get; set; }

    [Column("ServerID")]
    public int ServerId { get; set; }

    [Column("ProviderID")]
    public int ProviderId { get; set; }

    [Required]
    [StringLength(50)]
    public string ServiceName { get; set; }

    //[Column(TypeName = "ntext")]
    public string Comments { get; set; }

    public int? ServiceQuotaValue { get; set; }

    [Column("ClusterID")]
    public int? ClusterId { get; set; }

    [ForeignKey("ClusterId")]
    [InverseProperty("Services")]
    public virtual Cluster Cluster { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<GlobalDnsRecord> GlobalDnsRecords { get; set; } = new List<GlobalDnsRecord>();

    [ForeignKey("ProviderId")]
    [InverseProperty("Services")]
    public virtual Provider Provider { get; set; }

    [ForeignKey("ServerId")]
    [InverseProperty("Services")]
    public virtual Server Server { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

    [InverseProperty("Service")]
    public virtual ICollection<ServiceProperty> ServiceProperties { get; set; } = new List<ServiceProperty>();

    [InverseProperty("Service")]
    public virtual ICollection<StorageSpace> StorageSpaces { get; set; } = new List<StorageSpace>();

    [InverseProperty("Service")]
    public virtual ICollection<VirtualService> VirtualServices { get; set; } = new List<VirtualService>();

/*    [ForeignKey("ServiceId")]
    [InverseProperty("Services")]
    public virtual ICollection<Package> Packages { get; set; } = new List<Package>(); */
}
#endif