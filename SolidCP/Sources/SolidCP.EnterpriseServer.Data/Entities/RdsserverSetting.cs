#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class RdsserverSetting
{
    public int RdsServerId { get; set; }

    public string SettingsName { get; set; }

    public string PropertyName { get; set; }

    public string PropertyValue { get; set; }

    public bool ApplyUsers { get; set; }

    public bool ApplyAdministrators { get; set; }
}
#endif