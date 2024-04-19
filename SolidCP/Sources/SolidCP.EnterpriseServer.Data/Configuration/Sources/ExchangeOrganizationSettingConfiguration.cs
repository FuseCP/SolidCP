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

public partial class ExchangeOrganizationSettingConfiguration: Extensions.EntityTypeConfiguration<ExchangeOrganizationSetting>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ExchangeOrganizationSettingConfiguration(): base() { }
    public ExchangeOrganizationSettingConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasNoKey();

        HasIndex(e => e.ItemId, "ExchangeOrganizationSettingsIdx_ItemId");

        Property(e => e.SettingsName)
                .IsRequired()
                .HasMaxLength(100);
        Property(e => e.Xml).IsRequired();

        HasOne(d => d.Item).WithMany().HasForeignKey(d => d.ItemId);
    }
#endif
}
