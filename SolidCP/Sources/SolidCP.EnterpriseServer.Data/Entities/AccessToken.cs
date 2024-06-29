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
[Index("AccountId", Name = "AccessTokensIdx_AccountID")]
#endif
public partial class AccessToken
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public Guid AccessTokenGuid { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime ExpirationDate { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    public int ItemId { get; set; }

    public Base.HostedSolution.AccessTokenTypes TokenType { get; set; }

    [StringLength(100)]
#if NetCore
    [Unicode(false)]
#endif
    public string SmsResponse { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("AccessTokens")]
    public virtual ExchangeAccount Account { get; set; }
}
#endif