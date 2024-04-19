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

public partial class WebDavPortalUsersSettingConfiguration: Extensions.EntityTypeConfiguration<WebDavPortalUsersSetting>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public WebDavPortalUsersSettingConfiguration(): base() { }
    public WebDavPortalUsersSettingConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__WebDavPo__3214EC278AF5195E");

        HasIndex(e => e.AccountId, "WebDavPortalUsersSettingsIdx_AccountId");

        Property(e => e.Id).HasColumnName("ID");

        HasOne(d => d.Account).WithMany(p => p.WebDavPortalUsersSettings)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_WebDavPortalUsersSettings_UserId");
    }
#endif
}
