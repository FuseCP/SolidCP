#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class AccessToken
{
    public int Id { get; set; }

    public Guid AccessTokenGuid { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int AccountId { get; set; }

    public int ItemId { get; set; }

    public int TokenType { get; set; }

    public string SmsResponse { get; set; }

    public virtual ExchangeAccount Account { get; set; }
}
#endif