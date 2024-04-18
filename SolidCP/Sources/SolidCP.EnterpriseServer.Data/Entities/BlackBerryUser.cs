#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class BlackBerryUser
{
    public int BlackBerryUserId { get; set; }

    public int AccountId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual ExchangeAccount Account { get; set; }
}
#endif