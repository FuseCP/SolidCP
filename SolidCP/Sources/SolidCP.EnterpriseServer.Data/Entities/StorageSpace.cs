#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Index("ServerId", Name = "StorageSpacesIdx_ServerId")]
[Index("ServiceId", Name = "StorageSpacesIdx_ServiceId")]
public partial class StorageSpace
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(300)]
    [Unicode(false)]
    public string Name { get; set; }

    public int ServiceId { get; set; }

    public int ServerId { get; set; }

    public int LevelId { get; set; }

    [Required]
    [Unicode(false)]
    public string Path { get; set; }

    public bool IsShared { get; set; }

    [Unicode(false)]
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