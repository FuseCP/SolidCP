// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeOrganizationSsFolder
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public string Type { get; set; }

    public int StorageSpaceFolderId { get; set; }

    public virtual ExchangeOrganization Item { get; set; }

    public virtual StorageSpaceFolder StorageSpaceFolder { get; set; }
}
#endif