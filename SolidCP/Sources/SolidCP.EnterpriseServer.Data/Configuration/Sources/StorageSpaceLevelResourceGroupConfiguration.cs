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

public partial class StorageSpaceLevelResourceGroupConfiguration: Extensions.EntityTypeConfiguration<StorageSpaceLevelResourceGroup>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public StorageSpaceLevelResourceGroupConfiguration(): base() { }
    public StorageSpaceLevelResourceGroupConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__StorageS__3214EC07EBEBED98");

        HasIndex(e => e.GroupId, "StorageSpaceLevelResourceGroupsIdx_GroupId");

        HasIndex(e => e.LevelId, "StorageSpaceLevelResourceGroupsIdx_LevelId");

        HasOne(d => d.Group).WithMany(p => p.StorageSpaceLevelResourceGroups)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_StorageSpaceLevelResourceGroups_GroupId");

        HasOne(d => d.Level).WithMany(p => p.StorageSpaceLevelResourceGroups)
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("FK_StorageSpaceLevelResourceGroups_LevelId");
    }
#endif
}
