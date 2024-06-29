#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("RDSCertificates")]
public partial class RdsCertificate
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int ServiceId { get; set; }

    [Required]
    //[Column(TypeName = "ntext")]
    public string Content { get; set; }

    [Required]
    [StringLength(255)]
    public string Hash { get; set; }

    [Required]
    [StringLength(255)]
    public string FileName { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? ValidFrom { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }
}
#endif