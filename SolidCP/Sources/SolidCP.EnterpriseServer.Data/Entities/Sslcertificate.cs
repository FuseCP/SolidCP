#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Sslcertificate
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int SiteId { get; set; }

    public string FriendlyName { get; set; }

    public string Hostname { get; set; }

    public string DistinguishedName { get; set; }

    public string Csr { get; set; }

    public int? Csrlength { get; set; }

    public string Certificate { get; set; }

    public string Hash { get; set; }

    public bool? Installed { get; set; }

    public bool? IsRenewal { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string SerialNumber { get; set; }

    public string Pfx { get; set; }

    public int? PreviousId { get; set; }
}
#endif