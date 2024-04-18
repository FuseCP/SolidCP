#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class WebDavAccessToken
{
    public int Id { get; set; }

    public string FilePath { get; set; }

    public string AuthData { get; set; }

    public Guid AccessToken { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int AccountId { get; set; }

    public int ItemId { get; set; }

    public virtual ExchangeAccount Account { get; set; }
}
#endif