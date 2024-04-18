#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Rdsserver
{
    public int Id { get; set; }

    public int? ItemId { get; set; }

    public string Name { get; set; }

    public string FqdName { get; set; }

    public string Description { get; set; }

    public int? RdscollectionId { get; set; }

    public bool ConnectionEnabled { get; set; }

    public int? Controller { get; set; }

    public virtual Rdscollection Rdscollection { get; set; }
}
#endif