#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Crmuser
{
    public int CrmuserId { get; set; }

    public int AccountId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ChangedDate { get; set; }

    public Guid? CrmuserGuid { get; set; }

    public Guid? BusinessUnitId { get; set; }

    public int? Caltype { get; set; }

    public virtual ExchangeAccount Account { get; set; }
}
#endif