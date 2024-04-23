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
[Index("DomainId", Name = "DomainDnsRecordsIdx_DomainId")]
#endif
public partial class DomainDnsRecord
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int DomainId { get; set; }

    public int RecordType { get; set; }

    [StringLength(255)]
    public string DnsServer { get; set; }

    [StringLength(255)]
    public string Value { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [ForeignKey("DomainId")]
    [InverseProperty("DomainDnsRecords")]
    public virtual Domain Domain { get; set; }
}
#endif