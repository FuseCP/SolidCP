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
[Index("IpaddressId", Name = "GlobalDnsRecordsIdx_IPAddressID")]
[Index("PackageId", Name = "GlobalDnsRecordsIdx_PackageID")]
[Index("ServerId", Name = "GlobalDnsRecordsIdx_ServerID")]
[Index("ServiceId", Name = "GlobalDnsRecordsIdx_ServiceID")]
#endif
public partial class GlobalDnsRecord
{
    [Key]
    [Column("RecordID")]
    public int RecordId { get; set; }

    [Required]
    [StringLength(10)]
#if NetCore
    [Unicode(false)]
#endif
    public string RecordType { get; set; }

    [Required]
    [StringLength(50)]
    public string RecordName { get; set; }

    [Required]
    [StringLength(500)]
    public string RecordData { get; set; }

    [Column("MXPriority")]
    public int Mxpriority { get; set; }

    [Column("ServiceID")]
    public int? ServiceId { get; set; }

    [Column("ServerID")]
    public int? ServerId { get; set; }

    [Column("PackageID")]
    public int? PackageId { get; set; }

    [Column("IPAddressID")]
    public int? IpaddressId { get; set; }

    public int? SrvPriority { get; set; }

    public int? SrvWeight { get; set; }

    public int? SrvPort { get; set; }

    [ForeignKey("IpaddressId")]
    [InverseProperty("GlobalDnsRecords")]
    public virtual Ipaddress Ipaddress { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("GlobalDnsRecords")]
    public virtual Package Package { get; set; }

    [ForeignKey("ServerId")]
    [InverseProperty("GlobalDnsRecords")]
    public virtual Server Server { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("GlobalDnsRecords")]
    public virtual Service Service { get; set; }
}
#endif