#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeDeletedAccount
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [Column("OriginAT")]
    public int OriginAt { get; set; }

    [StringLength(255)]
    public string StoragePath { get; set; }

    [StringLength(128)]
    public string FolderName { get; set; }

    [StringLength(128)]
    public string FileName { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime ExpirationDate { get; set; }
}
#endif