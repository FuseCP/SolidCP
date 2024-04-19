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

public partial class RdscollectionSettingConfiguration: Extensions.EntityTypeConfiguration<RdscollectionSetting>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public RdscollectionSettingConfiguration(): base() { }
    public RdscollectionSettingConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        ToTable("RDSCollectionSettings");

        HasIndex(e => e.RdscollectionId, "RDSCollectionSettingsIdx_RDSCollectionId");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.AuthenticateUsingNla).HasColumnName("AuthenticateUsingNLA");
        Property(e => e.BrokenConnectionAction).HasMaxLength(20);
        Property(e => e.ClientDeviceRedirectionOptions).HasMaxLength(250);
        Property(e => e.EncryptionLevel).HasMaxLength(20);
        Property(e => e.RdeasyPrintDriverEnabled).HasColumnName("RDEasyPrintDriverEnabled");
        Property(e => e.RdscollectionId).HasColumnName("RDSCollectionId");
        Property(e => e.SecurityLayer).HasMaxLength(20);

        HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionSettings)
                .HasForeignKey(d => d.RdscollectionId)
                .HasConstraintName("FK_RDSCollectionSettings_RDSCollections");
    }
#endif
}
