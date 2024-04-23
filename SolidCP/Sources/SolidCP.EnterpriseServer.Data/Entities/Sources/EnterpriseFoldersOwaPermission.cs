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
[Index("AccountId", Name = "EnterpriseFoldersOwaPermissionsIdx_AccountID")]
[Index("FolderId", Name = "EnterpriseFoldersOwaPermissionsIdx_FolderID")]
#endif
public partial class EnterpriseFoldersOwaPermission
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Column("FolderID")]
    public int FolderId { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("EnterpriseFoldersOwaPermissions")]
    public virtual ExchangeAccount Account { get; set; }

    [ForeignKey("FolderId")]
    [InverseProperty("EnterpriseFoldersOwaPermissions")]
    public virtual EnterpriseFolder Folder { get; set; }
}
#endif