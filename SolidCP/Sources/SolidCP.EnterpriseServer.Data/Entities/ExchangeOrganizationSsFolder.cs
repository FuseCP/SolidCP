#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

#if NetCore
[Index("ItemId", Name = "ExchangeOrganizationSsFoldersIdx_ItemId")]
[Index("StorageSpaceFolderId", Name = "ExchangeOrganizationSsFoldersIdx_StorageSpaceFolderId")]
#endif
public partial class ExchangeOrganizationSsFolder
{
    [Key]
    public int Id { get; set; }

    public int ItemId { get; set; }

    [Required]
    [StringLength(100)]
#if NetCore
    [Unicode(false)]
#endif
    public string Type { get; set; }

    public int StorageSpaceFolderId { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ExchangeOrganizationSsFolders")]
    public virtual ExchangeOrganization Item { get; set; }

    [ForeignKey("StorageSpaceFolderId")]
    [InverseProperty("ExchangeOrganizationSsFolders")]
    public virtual StorageSpaceFolder StorageSpaceFolder { get; set; }
}
#endif