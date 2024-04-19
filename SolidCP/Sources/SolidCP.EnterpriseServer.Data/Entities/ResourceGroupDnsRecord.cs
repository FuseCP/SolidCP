// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ResourceGroupDnsRecord
{
    public int RecordId { get; set; }

    public int RecordOrder { get; set; }

    public int GroupId { get; set; }

    public string RecordType { get; set; }

    public string RecordName { get; set; }

    public string RecordData { get; set; }

    public int? Mxpriority { get; set; }

    public virtual ResourceGroup Group { get; set; }
}
#endif