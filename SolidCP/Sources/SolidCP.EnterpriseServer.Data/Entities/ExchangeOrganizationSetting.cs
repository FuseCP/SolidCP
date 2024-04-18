#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class ExchangeOrganizationSetting
{
    public int ItemId { get; set; }

    public string SettingsName { get; set; }

    public string Xml { get; set; }

    public virtual ExchangeOrganization Item { get; set; }
}
#endif