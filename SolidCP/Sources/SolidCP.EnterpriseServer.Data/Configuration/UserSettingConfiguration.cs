using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class UserSettingConfiguration: Extensions.EntityTypeConfiguration<UserSetting>
{

    public UserSettingConfiguration(): base() { }
    public UserSettingConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {

#if NetCore
        HasOne(d => d.User).WithMany(p => p.UserSettings).HasConstraintName("FK_UserSettings_Users");
#else
        HasRequired(d => d.User).WithMany(p => p.UserSettings);
#endif
    }
#endif
    }
