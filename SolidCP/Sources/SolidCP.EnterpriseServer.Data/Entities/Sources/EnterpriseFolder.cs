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
[Index("StorageSpaceFolderId", Name = "EnterpriseFoldersIdx_StorageSpaceFolderId")]
#endif
public partial class EnterpriseFolder
{
    [Key]
    [Column("EnterpriseFolderID")]
    public int EnterpriseFolderId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [StringLength(255)]
    public string FolderName { get; set; }

    public int FolderQuota { get; set; }

    [StringLength(255)]
    public string LocationDrive { get; set; }

    [StringLength(255)]
    public string HomeFolder { get; set; }

    [StringLength(255)]
    public string Domain { get; set; }

    public int? StorageSpaceFolderId { get; set; }

    [InverseProperty("Folder")]
    public virtual ICollection<EnterpriseFoldersOwaPermission> EnterpriseFoldersOwaPermissions { get; set; } = new List<EnterpriseFoldersOwaPermission>();

    [ForeignKey("StorageSpaceFolderId")]
    [InverseProperty("EnterpriseFolders")]
    public virtual StorageSpaceFolder StorageSpaceFolder { get; set; }
}
#endif