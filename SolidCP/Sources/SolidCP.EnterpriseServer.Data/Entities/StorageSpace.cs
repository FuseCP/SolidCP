#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class StorageSpace
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int ServiceId { get; set; }

    public int ServerId { get; set; }

    public int LevelId { get; set; }

    public string Path { get; set; }

    public bool IsShared { get; set; }

    public string UncPath { get; set; }

    public int FsrmQuotaType { get; set; }

    public long FsrmQuotaSizeBytes { get; set; }

    public bool IsDisabled { get; set; }

    public virtual Server Server { get; set; }

    public virtual Service Service { get; set; }

    public virtual ICollection<StorageSpaceFolder> StorageSpaceFolders { get; set; } = new List<StorageSpaceFolder>();
}
#endif