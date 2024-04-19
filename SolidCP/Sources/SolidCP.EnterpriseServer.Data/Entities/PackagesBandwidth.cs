// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class PackagesBandwidth
{
    public int PackageId { get; set; }

    public int GroupId { get; set; }

    public DateTime LogDate { get; set; }

    public long BytesSent { get; set; }

    public long BytesReceived { get; set; }

    public virtual ResourceGroup Group { get; set; }

    public virtual Package Package { get; set; }
}
#endif