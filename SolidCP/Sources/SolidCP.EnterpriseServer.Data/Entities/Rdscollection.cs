#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Rdscollection
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string DisplayName { get; set; }

    public virtual ICollection<RdscollectionSetting> RdscollectionSettings { get; set; } = new List<RdscollectionSetting>();

    public virtual ICollection<RdscollectionUser> RdscollectionUsers { get; set; } = new List<RdscollectionUser>();

    public virtual ICollection<Rdsmessage> Rdsmessages { get; set; } = new List<Rdsmessage>();

    public virtual ICollection<Rdsserver> Rdsservers { get; set; } = new List<Rdsserver>();
}
#endif