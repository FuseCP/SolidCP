#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class EnterpriseFolder
{
    public int EnterpriseFolderId { get; set; }

    public int ItemId { get; set; }

    public string FolderName { get; set; }

    public int FolderQuota { get; set; }

    public string LocationDrive { get; set; }

    public string HomeFolder { get; set; }

    public string Domain { get; set; }

    public int? StorageSpaceFolderId { get; set; }

    public virtual ICollection<EnterpriseFoldersOwaPermission> EnterpriseFoldersOwaPermissions { get; set; } = new List<EnterpriseFoldersOwaPermission>();

    public virtual StorageSpaceFolder StorageSpaceFolder { get; set; }
}
#endif