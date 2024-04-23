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

public partial class StorageSpaceLevel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(300)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [InverseProperty("Level")]
    public virtual ICollection<StorageSpaceLevelResourceGroup> StorageSpaceLevelResourceGroups { get; set; } = new List<StorageSpaceLevelResourceGroup>();
}
#endif