#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("CRMUsers")]
#if NetCore
[Index("AccountId", Name = "CRMUsersIdx_AccountID")]
#endif
public partial class CrmUser
{
    [Key]
    [Column("CRMUserID")]
    public int CrmUserId { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime ChangedDate { get; set; }

    [Column("CRMUserGuid")]
    public Guid? CrmUserGuid { get; set; }

    [Column("BusinessUnitID")]
    public Guid? BusinessUnitId { get; set; }

    [Column("CALType")]
    public int? CalType { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("CrmUsers")]
    public virtual ExchangeAccount Account { get; set; }
}
#endif