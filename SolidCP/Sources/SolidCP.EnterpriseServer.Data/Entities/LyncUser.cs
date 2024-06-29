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
[Index("LyncUserPlanId", Name = "LyncUsersIdx_LyncUserPlanID")]
#endif
public partial class LyncUser
{
    [Key]
    [Column("LyncUserID")]
    public int LyncUserId { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [Column("LyncUserPlanID")]
    public int LyncUserPlanId { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [StringLength(300)]
    public string SipAddress { get; set; }

    [ForeignKey("LyncUserPlanId")]
    [InverseProperty("LyncUsers")]
    public virtual LyncUserPlan LyncUserPlan { get; set; }
}
#endif