// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ServiceItemType
{
    public int ItemTypeId { get; set; }

    public int? GroupId { get; set; }

    public string DisplayName { get; set; }

    public string TypeName { get; set; }

    public int TypeOrder { get; set; }

    public bool? CalculateDiskspace { get; set; }

    public bool? CalculateBandwidth { get; set; }

    public bool? Suspendable { get; set; }

    public bool? Disposable { get; set; }

    public bool? Searchable { get; set; }

    public bool Importable { get; set; }

    public bool Backupable { get; set; }

    public virtual ResourceGroup Group { get; set; }

    public virtual ICollection<Quota> Quota { get; set; } = new List<Quota>();

    public virtual ICollection<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();
}
#endif