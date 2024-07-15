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
#if NetCore
[Index("AccountId", Name = "RDSCollectionUsersIdx_AccountID")]
[Index("RdsCollectionId", Name = "RDSCollectionUsersIdx_RDSCollectionId")]
#endif
public partial class RdsCollectionUser
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("RDSCollectionId")]
    public int RdsCollectionId { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("RdsCollectionUsers")]
    public virtual ExchangeAccount Account { get; set; }

    [ForeignKey("RdsCollectionId")]
    [InverseProperty("RdsCollectionUsers")]
    public virtual RdsCollection RdsCollection { get; set; }
}
#endif