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
[Index("GroupId", Name = "ResourceGroupDnsRecordsIdx_GroupID")]
#endif
public partial class ResourceGroupDnsRecord
{
    [Key]
    [Column("RecordID")]
    public int RecordId { get; set; }

    public int RecordOrder { get; set; }

    [Column("GroupID")]
    public int GroupId { get; set; }

    [Required]
    [StringLength(50)]
#if NetCore
    [Unicode(false)]
#endif
    public string RecordType { get; set; }

    [Required]
    [StringLength(50)]
    public string RecordName { get; set; }

    [Required]
    [StringLength(200)]
    public string RecordData { get; set; }

    [Column("MXPriority")]
    public int? Mxpriority { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("ResourceGroupDnsRecords")]
    public virtual ResourceGroup Group { get; set; }
}
#endif