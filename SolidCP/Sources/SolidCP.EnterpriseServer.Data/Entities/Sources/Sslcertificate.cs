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

[Table("SSLCertificates")]
#if NetCore
[Keyless]
#endif
public partial class Sslcertificate
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("SiteID")]
    public int SiteId { get; set; }

    [StringLength(255)]
    public string FriendlyName { get; set; }

    [StringLength(255)]
    public string Hostname { get; set; }

    [StringLength(500)]
    public string DistinguishedName { get; set; }

    [Column("CSR", TypeName = "ntext")]
    public string Csr { get; set; }

    [Column("CSRLength")]
    public int? Csrlength { get; set; }

    [Column(TypeName = "ntext")]
    public string Certificate { get; set; }

    [Column(TypeName = "ntext")]
    public string Hash { get; set; }

    public bool? Installed { get; set; }

    public bool? IsRenewal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ValidFrom { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [StringLength(250)]
    public string SerialNumber { get; set; }

    [Column(TypeName = "ntext")]
    public string Pfx { get; set; }

    public int? PreviousId { get; set; }
}
#endif