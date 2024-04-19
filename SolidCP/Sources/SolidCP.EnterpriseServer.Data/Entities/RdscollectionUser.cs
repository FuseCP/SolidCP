// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class RdscollectionUser
{
    public int Id { get; set; }

    public int RdscollectionId { get; set; }

    public int AccountId { get; set; }

    public virtual ExchangeAccount Account { get; set; }

    public virtual Rdscollection Rdscollection { get; set; }
}
#endif