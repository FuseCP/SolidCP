#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class StorageSpaceLevelResourceGroup
{
    public int Id { get; set; }

    public int LevelId { get; set; }

    public int GroupId { get; set; }

    public virtual ResourceGroup Group { get; set; }

    public virtual StorageSpaceLevel Level { get; set; }
}
#endif