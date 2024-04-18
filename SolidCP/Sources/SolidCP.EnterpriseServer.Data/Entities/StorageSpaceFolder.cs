#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Index("StorageSpaceId", Name = "StorageSpaceFoldersIdx_StorageSpaceId")]
public partial class StorageSpaceFolder
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(300)]
    [Unicode(false)]
    public string Name { get; set; }

    public int StorageSpaceId { get; set; }

    [Required]
    [Unicode(false)]
    public string Path { get; set; }

    [Unicode(false)]
    public string UncPath { get; set; }

    public bool IsShared { get; set; }

    public int FsrmQuotaType { get; set; }

    public long FsrmQuotaSizeBytes { get; set; }

    [InverseProperty("StorageSpaceFolder")]
    public virtual ICollection<EnterpriseFolder> EnterpriseFolders { get; set; } = new List<EnterpriseFolder>();

    [InverseProperty("StorageSpaceFolder")]
    public virtual ICollection<ExchangeOrganizationSsFolder> ExchangeOrganizationSsFolders { get; set; } = new List<ExchangeOrganizationSsFolder>();

    [ForeignKey("StorageSpaceId")]
    [InverseProperty("StorageSpaceFolders")]
    public virtual StorageSpace StorageSpace { get; set; }
}
#endif