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
[Index("AccountId", Name = "WebDavAccessTokensIdx_AccountID")]
#endif
public partial class WebDavAccessToken
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    public string FilePath { get; set; }

    [Required]
    public string AuthData { get; set; }

    public Guid AccessToken { get; set; }

    //[Column(TypeName = "datetime")]
    public DateTime ExpirationDate { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    public int ItemId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("WebDavAccessTokens")]
    public virtual ExchangeAccount Account { get; set; }
}
#endif