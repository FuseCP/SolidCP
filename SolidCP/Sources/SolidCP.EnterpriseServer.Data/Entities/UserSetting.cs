#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class UserSetting
{
    public int UserId { get; set; }

    public string SettingsName { get; set; }

    public string PropertyName { get; set; }

    public string PropertyValue { get; set; }

    public virtual User User { get; set; }
}
#endif