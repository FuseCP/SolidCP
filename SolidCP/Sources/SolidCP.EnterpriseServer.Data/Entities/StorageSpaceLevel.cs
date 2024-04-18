#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class StorageSpaceLevel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<StorageSpaceLevelResourceGroup> StorageSpaceLevelResourceGroups { get; set; } = new List<StorageSpaceLevelResourceGroup>();
}
#endif