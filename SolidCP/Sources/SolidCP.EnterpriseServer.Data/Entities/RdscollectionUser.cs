#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("RDSCollectionUsers")]
[Index("AccountId", Name = "RDSCollectionUsersIdx_AccountID")]
[Index("RdscollectionId", Name = "RDSCollectionUsersIdx_RDSCollectionId")]
public partial class RdscollectionUser
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("RDSCollectionId")]
    public int RdscollectionId { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("RdscollectionUsers")]
    public virtual ExchangeAccount Account { get; set; }

    [ForeignKey("RdscollectionId")]
    [InverseProperty("RdscollectionUsers")]
    public virtual Rdscollection Rdscollection { get; set; }
}
#endif