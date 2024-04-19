// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class StorageSpaceFolder
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int StorageSpaceId { get; set; }

    public string Path { get; set; }

    public string UncPath { get; set; }

    public bool IsShared { get; set; }

    public int FsrmQuotaType { get; set; }

    public long FsrmQuotaSizeBytes { get; set; }

    public virtual ICollection<EnterpriseFolder> EnterpriseFolders { get; set; } = new List<EnterpriseFolder>();

    public virtual ICollection<ExchangeOrganizationSsFolder> ExchangeOrganizationSsFolders { get; set; } = new List<ExchangeOrganizationSsFolder>();

    public virtual StorageSpace StorageSpace { get; set; }
}
#endif