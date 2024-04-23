// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities.Sources;

#if NetCore
[Index("GroupId", Name = "StorageSpaceLevelResourceGroupsIdx_GroupId")]
[Index("LevelId", Name = "StorageSpaceLevelResourceGroupsIdx_LevelId")]
#endif
public partial class StorageSpaceLevelResourceGroup
{
    [Key]
    public int Id { get; set; }

    public int LevelId { get; set; }

    public int GroupId { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("StorageSpaceLevelResourceGroups")]
    public virtual ResourceGroup Group { get; set; }

    [ForeignKey("LevelId")]
    [InverseProperty("StorageSpaceLevelResourceGroups")]
    public virtual StorageSpaceLevel Level { get; set; }
}
#endif