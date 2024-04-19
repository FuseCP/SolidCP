// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Rdsmessage
{
    public int Id { get; set; }

    public int RdscollectionId { get; set; }

    public string MessageText { get; set; }

    public string UserName { get; set; }

    public DateTime Date { get; set; }

    public virtual Rdscollection Rdscollection { get; set; }
}
#endif