// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class UserSettingConfiguration: EntityTypeConfiguration<UserSetting>
{

#if NetCore || NetFX
    public override void Configure() {
        HasOne(d => d.User).WithMany(p => p.UserSettings).HasConstraintName("FK_UserSettings_Users");
    }
#endif
}
