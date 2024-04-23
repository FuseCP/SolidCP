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
[Index("StorageSpaceId", Name = "StorageSpaceFoldersIdx_StorageSpaceId")]
#endif
public partial class StorageSpaceFolder
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(300)]
#if NetCore
    [Unicode(false)]
#endif
    public string Name { get; set; }

    public int StorageSpaceId { get; set; }

    [Required]
#if NetCore
    [Unicode(false)]
#endif
    public string Path { get; set; }

#if NetCore
    [Unicode(false)]
#endif
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