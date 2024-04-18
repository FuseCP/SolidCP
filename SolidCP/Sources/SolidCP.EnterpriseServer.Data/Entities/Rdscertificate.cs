#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Rdscertificate
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public string Content { get; set; }

    public string Hash { get; set; }

    public string FileName { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ExpiryDate { get; set; }
}
#endif