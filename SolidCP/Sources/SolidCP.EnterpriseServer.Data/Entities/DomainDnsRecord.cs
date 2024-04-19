// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class DomainDnsRecord
{
    public int Id { get; set; }

    public int DomainId { get; set; }

    public int RecordType { get; set; }

    public string DnsServer { get; set; }

    public string Value { get; set; }

    public DateTime? Date { get; set; }

    public virtual Domain Domain { get; set; }
}
#endif