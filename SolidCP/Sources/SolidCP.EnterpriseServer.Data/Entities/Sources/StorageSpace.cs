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
[Index("ServerId", Name = "StorageSpacesIdx_ServerId")]
[Index("ServiceId", Name = "StorageSpacesIdx_ServiceId")]
#endif
public partial class StorageSpace
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(300)]
#if NetCore
    [Unicode(false)]
#endif
    public string Name { get; set; }

    public int ServiceId { get; set; }

    public int ServerId { get; set; }

    public int LevelId { get; set; }

    [Required]
#if NetCore
    [Unicode(false)]
#endif
    public string Path { get; set; }

    public bool IsShared { get; set; }

#if NetCore
    [Unicode(false)]
#endif
    public string UncPath { get; set; }

    public int FsrmQuotaType { get; set; }

    public long FsrmQuotaSizeBytes { get; set; }

    public bool IsDisabled { get; set; }

    [ForeignKey("ServerId")]
    [InverseProperty("StorageSpaces")]
    public virtual Server Server { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("StorageSpaces")]
    public virtual Service Service { get; set; }

    [InverseProperty("StorageSpace")]
    public virtual ICollection<StorageSpaceFolder> StorageSpaceFolders { get; set; } = new List<StorageSpaceFolder>();
}
#endif