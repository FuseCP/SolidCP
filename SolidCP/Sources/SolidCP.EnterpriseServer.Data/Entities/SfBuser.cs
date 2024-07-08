#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("SfBUsers")]
public partial class SfBUser
{
    [Key]
    [Column("SfBUserID")]
    public int SfBUserId { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [Column("SfBUserPlanID")]
    public int SfBUserPlanId { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [StringLength(300)]
    public string SipAddress { get; set; }
}
#endif