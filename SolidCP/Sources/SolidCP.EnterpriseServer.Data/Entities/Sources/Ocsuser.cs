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

[Table("OCSUsers")]
public partial class Ocsuser
{
    [Key]
    [Column("OCSUserID")]
    public int OcsuserId { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [Required]
    [Column("InstanceID")]
    [StringLength(50)]
    public string InstanceId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }
}
#endif