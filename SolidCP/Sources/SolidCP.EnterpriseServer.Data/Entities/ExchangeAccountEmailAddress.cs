#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeAccountEmailAddress
{
    public int AddressId { get; set; }

    public int AccountId { get; set; }

    public string EmailAddress { get; set; }

    public virtual ExchangeAccount Account { get; set; }
}
#endif