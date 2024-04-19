// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#endif
#if NetFX
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class UserSettingConfiguration: Extensions.EntityTypeConfiguration<UserSetting>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public UserSettingConfiguration(): base() { }
    public UserSettingConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => new { e.UserId, e.SettingsName, e.PropertyName });

        Property(e => e.UserId).HasColumnName("UserID");
        Property(e => e.SettingsName).HasMaxLength(50);
        Property(e => e.PropertyName).HasMaxLength(50);
        Property(e => e.PropertyValue).HasColumnType("ntext");

        HasOne(d => d.User).WithMany(p => p.UserSettings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserSettings_Users");
    }
#endif
}
