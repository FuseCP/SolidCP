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
[Index("AccountId", Name = "BlackBerryUsersIdx_AccountId")]
#endif
public partial class BlackBerryUser
{
    [Key]
    public int BlackBerryUserId { get; set; }

    public int AccountId { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("BlackBerryUsers")]
    public virtual ExchangeAccount Account { get; set; }
}
#endif